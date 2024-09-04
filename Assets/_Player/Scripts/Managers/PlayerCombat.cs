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
    [SerializeField] public float launchHeight;
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

    public float parryRange;
    public Transform parryPoint;
    public AttackStats parryStats;

    public Transform slamDetectionPoint;
    public GameObject slamVFX;
    public float slamDelay;

    private void Start()
    {
        _sm = GetComponent<PlayerStateManager>();
        resources = GetComponent<PlayerResources>();
        movement = GetComponent<PlayerMovement>();
        Vector3 launchPosition = new Vector3(transform.position.x, transform.position.y + launchHeight, transform.position.z);
        yPosition = launchPosition.y;
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

            EnemyAI e = c.GetComponent<EnemyAI>();
            if (e != null)
            {
                e.SwitchState(new EnemyAirborne());
            }

            HitstopManager.Instance.TriggerHitstop(modeStats.hitstopAmnt, gameObject, c.gameObject);
            CinemachineShake.Instance.ShakeCamera(modeStats.shakeAmnt, modeStats.shakeAmnt);
            RumbleManager.instance.RumblePulse(.15f, .25f, .3f);
            transform.DOKill();
            Vector3 launchPosition = new Vector3(c.transform.position.x, transform.position.y + launchHeight, c.transform.position.z);
            c.transform.DOMove(launchPosition, launchDuration).SetEase(Ease.OutQuad);

            //if (c.GetComponent<EnemyHealth>() != null)
            //{
            //    c.GetComponent<EnemyHealth>().GetStunned(.2f);

            //}



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
            EnemyHealth enemy = c.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.SlamToGround();
            }

            Dummy dummy = c.GetComponent<Dummy>();
            if (dummy != null)
            {
                dummy.SlamToGround();
            }
        }
    }

    public void SlamToGround()
    {
        _sm.anim.speed = 0;
        StartCoroutine(PauseDelay());

        IEnumerator PauseDelay()
        {
            yield return new WaitForSeconds(slamDelay);
            _sm.anim.speed = 1;
            float groundYPosition = DetectGroundPosition();
            DetectAndSlamEnemies();
            // Move the enemy down to the ground
            transform.DOMoveY(groundYPosition, slamDuration).SetEase(Ease.InQuad).OnComplete(() =>
            {
                PlayerAudioManager.instance.SlamExplode(); Instantiate(slamVFX, transform.position, Quaternion.identity); _sm.SwitchState(new PlayerIdleState());
            });
        }

    }


    private float DetectGroundPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(slamDetectionPoint.position, Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.point.y + 1;
        }
        else
        {
            // Fallback if no ground detected (unlikely in a well-defined environment)
            return 0f;
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

        if (index < 1) return;
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
        pauseAttack = false;
    }

    public (Transform transform, float distance) ClosestEnemy()
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
            float closestDistance = Mathf.Sqrt(closestDistanceSqr); // Calculate the actual distance
            return (closestEnemy.transform, closestDistance);
        }

        return (null, 0f);
    }

    public void ParryEffect(GameObject c)
    {
        HitstopManager.Instance.TriggerHitstop(parryStats.hitstopAmnt, gameObject, c);
        CinemachineShake.Instance.ShakeCamera(parryStats.shakeAmnt, parryStats.shakeAmnt);
        CinemachineShake.Instance.ChangeFov(parryStats.zoomAmnt, parryStats.shakeDur);
        RumbleManager.instance.RumblePulse(.15f, .25f, .3f);
        transform.DOKill();
    }



}
