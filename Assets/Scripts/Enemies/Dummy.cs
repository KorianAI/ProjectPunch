using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dummy : MonoBehaviour, IDamageable, IMagnetisable
{
    public ParticleSystem particle;
    public bool canSpawn;
    public GameObject player;

    public GameObject dummy;

    public PlayerStateManager ps;

    bool takenDamage;

    public Animator anim;
    public GameObject pullVFX;

    public float xShake;

    public float slamDuration = 0.5f;
    public GameObject slamVFX;
    public Transform slamDetectionPoint;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void TakeDamage(float damage)
    {
        if (!takenDamage)
        {
            takenDamage = true;

            SpawnParticle();
            //transform.DOShakeScale(.2f, .1f, 10, 90);

            anim.SetTrigger("Hit");
            dummy.transform.DOShakePosition(.25f, new Vector3(xShake, 0f, 0f), 11, 90, false, false, ShakeRandomnessMode.Harmonic);
            StartCoroutine(ResetTakenDamage());
        }

        IEnumerator ResetTakenDamage()
        {
            yield return new WaitForSeconds(.2f);
            takenDamage = false;
        }
    }

    private void SpawnParticle()
    {
        if (canSpawn)
        {
            var em = particle.emission;
            var dur = particle.main.duration;

            em.enabled = true;
            particle.Play();

            canSpawn = false;
            Invoke("TurnOff", dur / 2);
            Invoke("UpdatePlayerScrap", .2f);
        }
    }

    private void UpdatePlayerScrap()
    {
        player.GetComponent<PlayerResources>().UpdateScrap(10);
    }

    void TurnOff()
    {
        var em = particle.emission;
        em.enabled = false;
        canSpawn = true;
    }

    public void Pull(PlayerStateManager player)
    {
        ps = player;
        pullVFX.SetActive(true);
        transform.DOMove(player.pullPosition.position, .5f).OnComplete(() => { player.pulling = false; pullVFX.SetActive(false); });  
        transform.DOShakeRotation(1, 15f, 10, 90);
        DOTween.To(() => player.playerCam.m_Lens.FieldOfView, x => player.playerCam.m_Lens.FieldOfView = x, 50, .25f);
    }

    public void Push(PlayerStateManager player)
    {

    }


    public void SlamToGround()
    {
        // Detect the ground position
        float groundYPosition = DetectGroundPosition();

        // Move the enemy down to the ground
        transform.DOMoveY(groundYPosition, slamDuration).SetEase(Ease.InQuad).OnComplete(() => { PlayerAudioManager.instance.SlamExplode(); Instantiate(slamVFX, transform.position, Quaternion.identity); });
    }


    private float DetectGroundPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(slamDetectionPoint.position, Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.point.y;
        }
        else
        {
            // Fallback if no ground detected (unlikely in a well-defined environment)
            return 0f;
        }
    }
}
