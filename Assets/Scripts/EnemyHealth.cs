using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZCameraShake;
using TreeEditor;

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
    [SerializeField] float currentArmour;

    public HealthBar healthBar;
    public HealthBar armourBar;

    public bool hasArmour;

    public Material[] mats;
    bool isFading;

    private void Start()
    {
        player = GameObject.Find("Player");
        ai = GetComponent<EnemyAI>();

        currentHealth = stats.health;
        currentArmour = stats.armour;

        healthBar.maxHealth = stats.health;
        healthBar.currentHealth = currentHealth;
        armourBar.currentHealth = currentArmour;
        armourBar.maxHealth = stats.armour;

        mats = GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;
    }

    public void TakeDamage(float damage)
    {
        if (!takenDamage)
        {
            takenDamage = true;

            if (hasArmour)
            {
                float remainingDamage = damage - currentArmour;
                Debug.Log(remainingDamage);
                currentArmour -= damage;
                armourBar.currentHealth = currentArmour;
                armourBar.DrawSlots();
                if (currentArmour <= 0)
                {
                    hasArmour = false;
                    if (remainingDamage > 0)
                    {
                        currentHealth -= remainingDamage;
                        healthBar.currentHealth = currentHealth;
                        healthBar.DrawSlots();
                    }
                }


            }

            else
            {
                currentHealth -= damage;
                healthBar.currentHealth = currentHealth;
                healthBar.DrawSlots();
            }


            SpawnParticle();
            transform.DOShakeScale(.2f, .1f, 10, 90);

            if (currentHealth <= 0 )
            {
                Die();
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

    private void Die()
    {
        ai.manager.enemies.Remove(ai);

        if (ai.manager.AliveEnemyCount() <= 0)
        {
            StartCoroutine(Finisher());
            StartCoroutine(DelayedFinisher());

            IEnumerator DelayedFinisher()
            {
                yield return new WaitForSeconds(.1f);
                ai.SwitchState(ai.deadState);
            }
        }
            
        else
        {
            ai.SwitchState(ai.deadState);
        }

        healthBar.gameObject.SetActive(false);
        if (ai.chase != null) { ai.StopCoroutine(ai.chase); }
        if (ai.patrol != null) { ai.StopCoroutine(ai.patrol); }
        StartCoroutine(DeathEffect());

        IEnumerator DeathEffect()
        {
            yield return new WaitForSeconds(5);
            Vector3 deadPos = new Vector3(transform.position.x, transform.position.y - .7f, transform.position.z);
            ai.agent.enabled = false;
            transform.DOMove(deadPos, 2f).onComplete = DestroyEnemy;
        }
    }

    void DestroyEnemy()
    {
        
        transform.DOKill();
        ai.transform.DOKill();
        Destroy(gameObject);
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

    public IEnumerator Finisher()
    {
        //finisher cam
        Time.timeScale = .5f;
        CameraManager.SwitchNonPlayerCam(ai.manager.finisherCam);
        PlayerStateManager.instance.anim.speed = .5f;
        yield return new WaitForSecondsRealtime(3);

        //door cam
        Time.timeScale = 1f;
        CameraManager.SwitchNonPlayerCam(ai.manager.exitDoorCam);
        ai.manager.exitDoorOpen.Play();
        yield return new WaitForSecondsRealtime(3);

        //return to player cam
        CameraManager.SwitchPlayerCam(PlayerStateManager.instance.playerCam);
        Time.timeScale = 1f;
        PlayerStateManager.instance.anim.speed = 1f;
    }

}
