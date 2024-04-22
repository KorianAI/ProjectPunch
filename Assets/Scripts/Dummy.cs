using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dummy : MonoBehaviour, IDamageable, IMagnetisable
{
    public ParticleSystem particle;
    public bool canSpawn;
    public GameObject player;

    public PlayerStateManager ps;

    bool takenDamage;

    public Animation anim;

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
            transform.DOShakeScale(.2f, .1f, 10, 90);

            anim.Play();

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

        transform.DOMove(player.pullPosition.position, 1f);
        transform.DOShakeRotation(1, 15f, 10, 90);
        DOTween.To(() => player.playerCam.m_Lens.FieldOfView, x => player.playerCam.m_Lens.FieldOfView = x, 50, .25f);
    }

    public void Push(PlayerStateManager player)
    {
        transform.DOMove(transform.position + player.orientation.forward * player.kbForce, 1f);
        transform.DOShakeRotation(1, 15f, 10, 90);
    }
}
