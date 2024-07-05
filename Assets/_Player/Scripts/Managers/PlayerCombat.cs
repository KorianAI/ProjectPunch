using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.VFX;
using Unity.VisualScripting;

public class PlayerCombat : MonoBehaviour
{
    PlayerStateManager _sm;
    PlayerResources resources;
    PlayerMovement movement;

    // attacking
    public Transform attackPoint;
    public float attackRange;
    public float enemyCheckRange;
    public LayerMask enemyLayer;
    public GameObject hitVFX;

    // launch
    [SerializeField] float launchHeight;
    [SerializeField] float launchDuration;
    [SerializeField] public float yPosition;

    // slam
    [SerializeField] float slamDuration;

    public float maxAirAttacks;
    public float currentAirAttacks;
    public bool airAtkGravity;

    public int attackIndex;
    public float pauseWindow;
    public float pauseLength;
    public float comboWindow;
    public float pauseWindowTime;

    public bool pauseAttack;
    Coroutine ComboWindowCoroutine;
    Coroutine PauseWindowCoroutine;



    private void Start()
    {
        _sm = GetComponent<PlayerStateManager>();
        resources = GetComponent<PlayerResources>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {

    }
    public void CheckForEnemies()
    {
        AttackStats modeStats = resources.mode.stats;
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider c in enemies)
        {

            c.GetComponent<IDamageable>().TakeDamage(modeStats.damage);

            _sm.attackHit = true;
            HitstopManager.Instance.TriggerHitstop(modeStats.hitstopAmnt, gameObject, c.gameObject);
            CinemachineShake.Instance.ShakeCamera(modeStats.shakeAmnt, modeStats.shakeAmnt);
            CinemachineShake.Instance.ChangeFov(modeStats.zoomAmnt, modeStats.shakeDur);
            RumbleManager.instance.RumblePulse(.15f, .25f, .3f);
            transform.DOKill();         

            GameObject hitParticle = Instantiate(hitVFX, c.transform);

            if (c.GetComponent<EnemyHealth>() != null)
            {
                c.GetComponent<EnemyHealth>().GetStunned(.2f);

            }

        }

        if (enemies.Length > 0)
        {
            PlayerAudioManager.instance.BaseAttackMetallic();
        }
    }

    public void Knockup(float type)
    {
        AttackStats modeStats = resources.mode.stats;
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider c in enemies)
        {
            HitstopManager.Instance.TriggerHitstop(modeStats.hitstopAmnt, gameObject, c.gameObject);
            CinemachineShake.Instance.ShakeCamera(modeStats.shakeAmnt, modeStats.shakeAmnt);
            RumbleManager.instance.RumblePulse(.15f, .25f, .3f);
            transform.DOKill();
            Vector3 launchPosition = new Vector3(c.transform.position.x, transform.position.y + launchHeight, c.transform.position.z);
            c.transform.DOMove(launchPosition, launchDuration).SetEase(Ease.OutQuad);

            if (c.GetComponent<EnemyHealth>() != null)
            {
                c.GetComponent<EnemyHealth>().GetStunned(.2f);

            }

        }

        if (type == 2)
        {
            Vector3 launchPosition = new Vector3(transform.position.x, transform.position.y + launchHeight, transform.position.z);
            yPosition = launchPosition.y;
            transform.DOMove(launchPosition, launchDuration).SetEase(Ease.OutQuad);
            movement.JumpEffect();
        }
    }

    private void DetectAndSlamEnemies()
    {
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (var c in enemies)
        {
            Dummy enemy = c.GetComponent<Dummy>();
            if (enemy != null)
            {
                enemy.SlamToGround();
            }
        }
    }

    public void AirAttackIncrement(float amnt)
    {
        currentAirAttacks += amnt;
        if (currentAirAttacks >= maxAirAttacks)
        {
            airAtkGravity = true;
        }
    }

    public void ResetAirGrav()
    {
        airAtkGravity = false;
        currentAirAttacks = 0;
        movement.airDashAmount = 0;
    }

    public void SaveAtkIndex(int index)
    {
        attackIndex = index;
        if (ComboWindowCoroutine != null)
        {
            StopCoroutine(ComboWindowCoroutine);
        }        
        ComboWindowCoroutine = StartCoroutine(ClearAtkIndex());
        
        if (PauseWindowCoroutine != null)
        {
            StopCoroutine(PauseWindowCoroutine);
        }

        PauseWindowCoroutine = StartCoroutine(ComboPause());
        
    }

    IEnumerator ComboPause()
    {
        yield return new WaitForSeconds(pauseLength);
        pauseAttack = true;
        yield return new WaitForSeconds(pauseWindow);
        pauseAttack = false;
    }

    IEnumerator ClearAtkIndex()
    {
        yield return new WaitForSeconds(comboWindow);
        attackIndex = 0;
    }

    public Vector3 ClosestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, enemyCheckRange, enemyLayer);
        Collider closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            float dSqrToTarget = directionToEnemy.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            return closestEnemy.transform.position;
        }
        return Vector3.zero;
    }

}
