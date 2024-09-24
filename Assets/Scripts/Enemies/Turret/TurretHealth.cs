using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretHealth : MonoBehaviour, IDamageable, IMagnetisable, IKnockback
{
    public ParticleSystem scrapParticle;
    public bool canSpawn;
    public GameObject player;

    bool takenDamage = false;

    public PlayerStateManager ps;


    [SerializeField] TurretAI ai;
    [SerializeField] EnemySO stats;
    [SerializeField] public float currentHealth;
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

    // Nails
    public bool nailImpaled;
    public ConcentratedNail nail;
    public float dir;

    public PlayerStateManager sm;

    // slam
    public float slamDuration = 0.5f;
    public GameObject slamVFX;
    public Transform slamDetectionPoint;

    public ParticleSystem smokevfx;
    public ActivateRailMovement arm;


    private void Start()
    {
        player = GameObject.Find("Player");
        sm = player.GetComponent<PlayerStateManager>();
        ai = GetComponent<TurretAI>();
        arm = GetComponent<ActivateRailMovement>();

        currentHealth = stats.health;
        currentArmour = stats.armour;

        healthBar.maxValue = stats.health;
        healthBar.currentValue = currentHealth;
        armourBar.currentValue = currentArmour;
        armourBar.maxValue = stats.armour;

        //mats = GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;

        healthBars = GetComponentInChildren<HealthBars>();
        audioManager = GetComponent<EnemyAudioManager>();
    }

    public void TakeDamage(float damage, bool ranged)
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
                EnemySFX.instance.SFX_TurretHit();
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
                EnemySFX.instance.SFX_TurretHit();
                healthBar.currentValue = currentHealth;
                healthBar.DrawSlots();
            }

            //if (ai.inAir)
            //{
            //    ai.SwitchState(new EnemyAirborne());
            //}

            SpawnScrapParticle();
            transform.DOShakeScale(.2f, .1f, 10, 90);

            if (currentHealth <= 0)
            {
                Die();
            }

            //else
            //{
                StartCoroutine(ResetTakenDamage());
            //}
        }

        IEnumerator ResetTakenDamage()
        {
            yield return new WaitForSeconds(.2f);
            takenDamage = false;
        }
    }

    private void Die()
    {
        ai.dead = true;
        
        sm.tl.CancelLock();
        healthBars.HideBars();
        smokevfx.Play();
        EnemySFX.instance.SFX_TurretDestroyed();

        ai.SwitchState(new TurretDead());
        healthBar.gameObject.SetActive(false);
        armourBar.gameObject.SetActive(false);
        ai.chargeVFX.SetActive(false);
        ai.fireVFX.enabled = false;
        ai.targetLine.enabled = false;

        Targetable t = GetComponent<Targetable>();
        if (t != null)
        {
            t.untargetable = true;
        }

        if (ai.dotInstance != null)
        {
            Destroy(ai.dotInstance);
        }

        if (ai.manager)
        {
            ai.manager.turrets.Remove(ai);
        }

        StartCoroutine(DeathEffect());

        IEnumerator DeathEffect()
        {
            if (ai.deathFirePoint != null) //set targeting and fire a projectile
            {
                arm.railCam.gameObject.SetActive(true); //change to cam perspective
                ai.turretHead.transform.DOLookAt(ai.deathFirePoint.position, .5f); //look at fire point

                yield return new WaitForSeconds(.5f);

                ai.targetLine.SetPosition(0, ai.lineStartPos.transform.position);
                ai.targetLine.SetPosition(1, ai.deathFirePoint.position);

                yield return new WaitForSeconds(.5f);

                GameObject projectileGo = Instantiate(ai.projectile, ai.lineStartPos.transform.position, Quaternion.identity);
                Vector3 direction = (ai.deathFirePoint.position - ai.lineStartPos.transform.position).normalized;
                projectileGo.GetComponent<Rigidbody>().velocity = direction * ai.projectileForce;

                ai.enabled = false;

                yield return new WaitForSeconds(.6f);

                for (int i = 0; i < 3; i ++)
                {
                    Instantiate(slamVFX, ai.deathFirePoint.position, Quaternion.identity);
                    RumbleManager.instance.RumblePulse(.10f, .5f, .10f);
                    yield return new WaitForSeconds(.2f);
                }

                yield return new WaitForSeconds(.2f);

                arm.Defeated();

                yield return new WaitForSeconds(1.2f);

                arm.ResetCams();
            }

            //Vector3 deadPos = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);
            //transform.DOMove(deadPos, 1f).onComplete = DestroyEnemy;
            DestroyEnemy();
        }
    }

    public EnemyAI NearestEnemy()
    {
        if (ai.manager)
        {
            EnemyAI closestEnemy = null;
            float closestDistanceSqr = Mathf.Infinity; // Start with a large number

            foreach (EnemyAI enemy in ai.manager.enemies)
            {
                if (enemy == ai) { continue; }

                Vector3 directionToEnemy = enemy.transform.position - transform.position;
                float dSqrToTarget = directionToEnemy.sqrMagnitude;

                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }
        else
        {
            return null;
        }
    }

    void DestroyEnemy()
    {
        transform.DOKill();
        ai.transform.DOKill();
        Destroy(GetComponent<Targetable>());
        Destroy(ai.dotInstance);
        Destroy(ai.targetLine);
        //Destroy(gameObject);
    }


    private void SpawnScrapParticle()
    {
        if (canSpawn)
        {
            var em = scrapParticle.emission;
            var dur = scrapParticle.main.duration;

            em.enabled = true;
            scrapParticle.Play();

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
        var em = scrapParticle.emission;
        em.enabled = false;
        canSpawn = true;
    }

    public void Pull(PlayerStateManager player)
    {
        ////if (hasArmour) { return; }
        //ps = player;
        //if (ps.ih.inputDir.y < -.5f && nail != null)
        //{
        //    Debug.Log("pulled the nail out lel");
        //    nail.ReturnToSpawn();
        //    nail = null;
        //    nailImpaled = false;
        //    ps.pulling = false;
        //}

        //else
        //{
        //    transform.DOMove(player.pullPosition.position, 1f).OnComplete(() => { ps.pulling = false; });
        //}
    }

    public void Push(PlayerStateManager player)
    {
        //if (hasArmour) { return; }
        //transform.DOMove(transform.position + player.orientation.forward, 1f);
        //GetStunned(1);
        ////transform.DOShakeRotation(1, 15f, 10, 90);
    }

    public void GetStunned(float stunLength)
    {
        if (hasArmour) { return; }
        //ai.enemy.anim.SetBool("Stunned", true);
        //ai.enemy.agent.isStopped = true;
        //ai.SwitchState(new EnemyStunned());
        stunnedVFX.SetActive(true);
        StartCoroutine(ResetStun(stunLength));
    }

    IEnumerator ResetStun(float stunLength)
    {
        yield return new WaitForSeconds(stunLength);
        //ai.enemy.anim.SetBool("Stunned", false);
        stunnedVFX.SetActive(false);
        yield return new WaitForSeconds(.5f);

        //ai.enemy.agent.isStopped = false;
        //ai.SwitchState(new EnemyAttack());
    }

    public void Knockback(float distance, Transform attacker, float length)
    {
        transform.DOMove(transform.position += attacker.forward * distance, 3f);
    }

    public IEnumerator Finisher()
    {
        //finisher cam
        Time.timeScale = .5f;
        //CameraManager.SwitchNonPlayerCam(ai.manager.finisherCam);
        PlayerStateManager.instance.anim.speed = .5f;
        yield return new WaitForSecondsRealtime(4);

        //door cam
        Time.timeScale = 1f;
        //CameraManager.SwitchNonPlayerCam(ai.manager.exitDoorCam);
        //ai.manager.exitDoorOpen.Play();
        yield return new WaitForSecondsRealtime(4);

        //return to collision cam
        CameraManager.SwitchPlayerCam(PlayerStateManager.instance.playerCam);
        PlayerStateManager.instance.anim.speed = 1f;
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
