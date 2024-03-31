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
    [SerializeField] EnemySO stats;
    [SerializeField] float currentHealth;

    public HealthBar healthBar;

    private void Start()
    {
        player = GameObject.Find("Player");
        ai = GetComponent<EnemyAI>();

        currentHealth = stats.health;

        healthBar.maxHealth = stats.health;
        healthBar.currentHealth = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!takenDamage)
        {
            takenDamage = true;
            currentHealth -= damage;
            healthBar.currentHealth = currentHealth;
            healthBar.DrawSlots();

            SpawnParticle();
            transform.DOShakeScale(.2f, .1f, 10, 90);

            if (currentHealth <= 0 )
            {
                ai.SwitchState(ai.deadState);
            }

            else
            {

                StartCoroutine(ResetTakenDamage());
            }

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
        ai.SwitchState(ai.stunnedState);
        StartCoroutine(ResetStun(stunLength));
    }

    IEnumerator ResetStun(float stunLength)
    {
        yield return new WaitForSeconds(stunLength);
        ai.enemy.anim.SetBool("Stunned", false);
        yield return new WaitForSeconds(.5f);

        if (ai.currentState == ai.deadState) yield return null;
        ai.enemy.agent.isStopped = false;
        ai.SwitchState(ai.attackState);
    }

    public void Knockback(float distance, Transform attacker)
    {
        transform.DOMove(transform.position += attacker.forward * distance, 3f);
    }
}
