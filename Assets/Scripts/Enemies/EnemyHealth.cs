using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] GameObject shatterVFX;

    public SlotManager healthBar;
    public SlotManager armourBar;

    public bool hasArmour;

    public GameObject stunnedVFX;

    public Material[] mats;
    bool isFading;

    HealthBars healthBars;

    EnemyAudioManager audioManager;

    private void Start()
    {
        player = GameObject.Find("Player");
        ai = GetComponent<EnemyAI>();

        currentHealth = stats.health;
        currentArmour = stats.armour;

        healthBar.maxValue = stats.health;
        healthBar.currentValue = currentHealth;
        armourBar.currentValue = currentArmour;
        armourBar.maxValue = stats.armour;

        mats = GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;

        healthBars = GetComponentInChildren<HealthBars>();

        audioManager = GetComponent<EnemyAudioManager>();
    }

    public void TakeDamage(float damage)
    {
        if (!takenDamage)
        {
            takenDamage = true;

            healthBars.ShowBarsAttacked();

            //audioManager.BaseAttack();

            if (hasArmour)
            {
                float remainingDamage = damage - currentArmour;
                //Debug.Log(remainingDamage);
                currentArmour -= damage;
                armourBar.currentValue = currentArmour;
                armourBar.DrawSlots();
                if (currentArmour <= 0)
                {
                    Instantiate(shatterVFX, transform.position, Quaternion.identity);
                    hasArmour = false;
                    if (remainingDamage > 0)
                    {
                        currentHealth -= remainingDamage;
                        healthBar.currentValue = currentHealth;
                        healthBar.DrawSlots();
                    }
                }
            }

            else
            {
                currentHealth -= damage;
                healthBar.currentValue = currentHealth;
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
        healthBars.HideBars();

        ai.manager.enemies.Remove(ai);

        player.GetComponent<TargetLock>().currentTarget = null;
        player.GetComponent<TargetLock>().isTargeting = false;
        player.GetComponent<TargetLock>().lastTargetTag = null;

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

        if (ai.manager.chosenEnemy == ai) { ai.manager.RandomEnemy(); }
        ai.enabled = false;
        healthBar.gameObject.SetActive(false);
        armourBar.gameObject.SetActive(false);
        //if (ai.chase != null) { ai.StopCoroutine(ai.chase); }
        //if (ai.patrol != null) { ai.StopCoroutine(ai.patrol); }
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
        if (hasArmour) { return; }
        ps = player;
        
        transform.DOMove(player.pullPosition.position, 1f).OnComplete(() => { player.canAttack = true; });
        GetStunned(1);
        //transform.DOShakeRotation(1, 15f, 10, 90);
        DOTween.To(() => player.playerCam.m_Lens.FieldOfView, x => player.playerCam.m_Lens.FieldOfView = x, 50, .25f);
    }


    public void Push(PlayerStateManager player)
    {
        if (hasArmour) { return; }
        transform.DOMove(transform.position + player.orientation.forward, 1f);
        GetStunned(1);
        //transform.DOShakeRotation(1, 15f, 10, 90);
    }

    public void GetStunned(float stunLength)
    {
        if (hasArmour) { return; }
        ai.enemy.anim.SetBool("Stunned", true);
        ai.enemy.agent.isStopped = true;    
        ai.SwitchState(ai.stunnedState);
        stunnedVFX.SetActive(true);
        StartCoroutine(ResetStun(stunLength));
    }

    IEnumerator ResetStun(float stunLength)
    {
        yield return new WaitForSeconds(stunLength);
        ai.enemy.anim.SetBool("Stunned", false);
        stunnedVFX.SetActive(false);
        yield return new WaitForSeconds(.5f);

        if (ai.currentState == ai.deadState) yield return null;
        ai.enemy.agent.isStopped = false;
        ai.SwitchState(ai.attackState);
    }

    public void Knockback(float distance, Transform attacker, float length)
    {
        transform.DOMove(transform.position += attacker.forward * distance, 3f);
    }

    public IEnumerator Finisher()
    {
        //finisher cam
        Time.timeScale = .5f;
        CameraManager.SwitchNonPlayerCam(ai.manager.finisherCam);
        PlayerStateManager.instance.anim.speed = .5f;
        yield return new WaitForSecondsRealtime(4);

        //door cam
        Time.timeScale = 1f;
        CameraManager.SwitchNonPlayerCam(ai.manager.exitDoorCam);
        ai.manager.exitDoorOpen.Play();
        yield return new WaitForSecondsRealtime(4);

        //return to collision cam
        CameraManager.SwitchPlayerCam(PlayerStateManager.instance.playerCam);
        PlayerStateManager.instance.anim.speed = 1f;
    }

}