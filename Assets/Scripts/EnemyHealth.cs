using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZCameraShake;

public class EnemyHealth : MonoBehaviour, IDamageable, IMagnetisable, IKnockback
{
    public ParticleSystem particle;
    public bool canSpawn;
    public GameObject player;

    bool takenDamage;

    public PlayerStateManager ps;

    [SerializeField] EnemyAI ai;

    private void Start()
    {
        player = GameObject.Find("Player");
        ai = GetComponent<EnemyAI>();
    }

    public void TakeDamage(float damage)
    {
        if (!takenDamage)
        {
            takenDamage = true;
            Debug.Log("Owwww u hit: " + this.gameObject);
            SpawnParticle();
            transform.DOShakeScale(1, .1f, 10, 90);
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
            Invoke("TurnOff", dur /2);
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
        GetStunned(1);
        transform.DOShakeRotation(1, 15f, 10, 90);
        DOTween.To(() => player.playerCam.m_Lens.FieldOfView, x => player.playerCam.m_Lens.FieldOfView = x, 50, .25f);
    }


    public void Push(PlayerStateManager player)
    {
        transform.DOMove(transform.position + player.orientation.forward * player.kbForce, 1f);
        GetStunned(1);
        transform.DOShakeRotation(1, 15f, 10, 90);
    }

    public void GetStunned(float stunLength)
    {
        ai.enemy.anim.SetBool("Stunned", true);
        ai.enemy.agent.isStopped = true;
        StartCoroutine(ResetStun(stunLength));
    }

    IEnumerator ResetStun(float stunLength)
    {
        yield return new WaitForSeconds(stunLength);
        ai.enemy.anim.SetBool("Stunned", false);
        yield return new WaitForSeconds(.5f);
        ai.enemy.agent.isStopped = false;
    }

    public void Knockback(float distance, Transform attacker)
    {
        transform.DOMove(transform.position += attacker.forward * distance, 3f);
    }
}
