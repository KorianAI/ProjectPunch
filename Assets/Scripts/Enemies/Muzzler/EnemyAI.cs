using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public EnemyState currentState { get; set; }

    [Header("References")]
    public CombatManager manager;
    public EnemyInfo enemy;
    public NavMeshAgent agent;
    public GameObject playerPos;
    public GameObject enemyVisuals;
    public Rigidbody rb;

    [Header("Conditions")]
    public bool aggro;
    public bool inAttackRange;
    public bool attackToken;
    public bool circleToken;
    public bool rePositioning;
    public bool available;
    public bool inAir;
    public float enemyHeight;
    public LayerMask whatIsGround;

    public Coroutine circle;
    public Coroutine chase;
    public bool preparingToChase;

    public float remainingDistance;
    public float circleRadius;
    public float minRadius;

    public float damageRadius;

    public Vector3 debugDestination;
    NavMeshHit hit;

    public EnemyAudioManager audioManager;

    public string debugState;

    private void Start()
    {
        playerPos = PlayerStateManager.instance.gameObject;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(enemy.stats.moveSpeed, enemy.stats.moveSpeed + 2);
        enemy.ai = this;
        audioManager = GetComponent<EnemyAudioManager>();

        currentState = new EnemyIdle();
        currentState.EnterState(this);
    }

    public void SwitchState(EnemyState _state)
    {
        _state.EnterState(this);
        currentState = _state;
        _state.ExitState(this);

    }

    private void Update()
    {
        IsGrounded();
        currentState.FrameUpdate(this);
        debugState = currentState.ToString();
        debugDestination = agent.destination;;
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    public void IsGrounded()
    {
        RaycastHit debugHit;
        bool groundRaycast = Physics.Raycast(transform.position, Vector3.down, out debugHit, enemyHeight * 0.5f + 0.2f, whatIsGround);
        if (groundRaycast)
        {
            inAir = false;
        }

        if (!groundRaycast)
        {
            inAir = true;
        }


    }

    public bool InAttackRange()
    {
        inAttackRange = Physics.CheckSphere(transform.position, enemy.stats.patrolRange, enemy.whatIsPlayer);
        return inAttackRange;
    }

    public Vector3 GetRandomPointAroundPlayer(Vector3 center, float radius)
    {       
        Vector3 randomDirection = Vector3.zero;
        bool foundValidPoint = false;

        while (!foundValidPoint)
        {
            randomDirection = Random.insideUnitSphere * radius;
            randomDirection += center;

            // Check the distance between the random point and the collision
            if (Vector3.Distance(randomDirection, center) >= minRadius)
            {
                // Check if the random point is on the NavMesh
                if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
                {
                    foundValidPoint = true;
                }
            }
        }

        return hit.position;
    }

    public void CheckForPlayer()
    {
        Collider[] enemies = Physics.OverlapSphere(enemy.attackPoint.position, enemy.stats.range, enemy.whatIsPlayer);
        if (enemies.Length <= 0) { return; }
        foreach (Collider c in enemies)
        {
            c.GetComponent<IDamageable>().TakeDamage(enemy.stats.damage);
            EnemySFX.instance.SFX_MuzzlerKick();
            GameObject hitVFX = Instantiate(enemy.hitVFX, enemy.attackPoint);
            //c.GetComponent<IKnockback>().Knockback(1.5f, orientation);
            //RumbleManager.instance.RumblePulse(.25f, 1f, .25f);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(enemy.transform.position, enemy.stats.patrolRange);
    }
}