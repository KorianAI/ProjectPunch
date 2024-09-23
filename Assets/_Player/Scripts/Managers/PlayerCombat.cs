using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.VFX;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class PlayerCombat : MonoBehaviour
{
    public PlayerStateManager _sm;
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

    public Transform aoePoint;
    public GameObject explosionVFX;
    public GameObject overdriveExVFX;
    public float aoeRange;
    public AttackStats aoeStats;

    public AK.Wwise.Event playSFX_BFGImpact;
    public AK.Wwise.Event playSFX_RSImpact;

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
        AttackStats modeStats = new AttackStats();
        if (resources.scrapShift)
        {
            modeStats = resources.shift.stats;
        }
        else
        {
            modeStats = resources.mode.stats;
        }

        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider c in enemies)
        {
            c.GetComponent<IDamageable>().TakeDamage(modeStats.damage);
            if (resources.scrapShift)
            {
                PlaySFX_RSImpact();
            }

            else
            {
                PlaySFX_BFGImpact();
            }
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
                transform.DOKill();

                Vector3 launchPosition = new Vector3(c.transform.position.x, transform.position.y + launchHeight, c.transform.position.z);
                c.transform.DOMove(launchPosition, launchDuration).SetEase(Ease.OutQuad);
            }

            HitstopManager.Instance.TriggerHitstop(modeStats.hitstopAmnt, gameObject, c.gameObject);
            CinemachineShake.Instance.ShakeCamera(modeStats.shakeAmnt, modeStats.shakeAmnt);
            RumbleManager.instance.RumblePulse(.15f, .25f, .3f);
        }

        if (type == 2 || type == 3 && _sm.attackHit)
        {
            Vector3 launchPosition = new Vector3(transform.position.x, transform.position.y + launchHeight, transform.position.z);
            yPosition = launchPosition.y;
            transform.DOMove(launchPosition, launchDuration).SetEase(Ease.OutQuad);
            if (type == 3)
            {
                _sm.SwitchState(new PlayerLauncher());
            }
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
        _sm.anim.SetBool("AirAttack", true);    
        StartCoroutine(PauseDelay());

        IEnumerator PauseDelay()
        {
            yield return new WaitForSeconds(slamDelay);
            _sm.anim.speed = 1;
            float groundYPosition = DetectGroundPosition();
            DetectAndSlamEnemies();
            transform.DOKill();

            if (resources.shift.overdrive)
            {
                movement.DashEffect(movement.rsxDashPrefab, new Vector3(0, 0, 90));
                transform.DOMoveY(groundYPosition, 0.05f).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    movement.grounded = true;
                    PlayerAudioManager.instance.SlamExplode(); _sm.SwitchState(new PlayerIdleState());  //Instantiate(slamVFX, transform.position, Quaternion.identity);
                });
            }

            else
            {
                transform.DOMoveY(groundYPosition, slamDuration).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    movement.grounded = true;
                    PlayerAudioManager.instance.SlamExplode(); Instantiate(slamVFX, transform.position, Quaternion.identity); _sm.SwitchState(new PlayerIdleState());
                });
            }
            // Move the enemy down to the ground

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

    public void AOEKnockback()
    {
        float range = 0;
        GameObject eVFX = null;
        if (resources.shift.overdrive) { range = aoeRange * 2;  eVFX = Instantiate(overdriveExVFX, aoePoint.transform.position, Quaternion.identity); }
        else { range = aoeRange;  eVFX = Instantiate(explosionVFX, aoePoint.transform.position, Quaternion.identity); }

        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, range, enemyLayer);
        foreach (Collider c in enemies)
        {
            c.GetComponent<IDamageable>().TakeDamage(aoeStats.damage);
            _sm.attackHit = true;
            HitstopManager.Instance.TriggerHitstop(aoeStats.hitstopAmnt, gameObject, c.gameObject);
            CinemachineShake.Instance.ShakeCamera(aoeStats.shakeAmnt, aoeStats.shakeAmnt);
            CinemachineShake.Instance.ChangeFov(aoeStats.zoomAmnt, aoeStats.shakeDur);
            RumbleManager.instance.RumblePulse(.15f, .25f, .3f);
            transform.DOKill();
        }

       // Destroy(eVFX, 1);
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
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null && !ai.available) { continue; }


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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(aoePoint.position, aoeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aoePoint.position, aoeRange * 2);
    }

    public void PlaySFX_BFGImpact()
    {
        playSFX_BFGImpact.Post(gameObject);
    }

    public void PlaySFX_RSImpact()
    {
        playSFX_RSImpact.Post(gameObject);
    }
}
