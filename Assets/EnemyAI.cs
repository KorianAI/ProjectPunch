using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum DebugState
    {
        Idle,
        Chase,
        Attack,
        Circle,
        Stunned,
        Dead
    }

    public EnemyState currentState { get; set; }
    public DebugState debugState;

    // states
    public EnemyIdle idleState = new EnemyIdle();
    public EnemyChase chaseState = new EnemyChase();
    public EnemyAttack attackState = new EnemyAttack();
    public EnemyCircle circleState = new EnemyCircle();
    public EnemyStunned stunnedState = new EnemyStunned();
    public EnemyDead deadState = new EnemyDead();

    [Header("References")]
    public CombatManager manager;
    public EnemyInfo enemy;
    public NavMeshAgent agent;
    public GameObject playerPos;

    [Header("Conditions")]
    public bool aggro;
    public bool inAttackRange;
    public bool permissionToAttack;
    public bool rePositioning;
    public bool available;

    public Coroutine patrol;
    public Coroutine chase;
    public bool preparingToChase;

    public float remainingDistance;
    public float circleRadius;
    public float minRadius;

    public float damageRadius;

    public Vector3 debugDestination;
    NavMeshHit hit;


    private void Start()
    {
        playerPos = PlayerStateManager.instance.gameObject;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(enemy.stats.moveSpeed - 2, enemy.stats.moveSpeed);
        enemy.ai = this;

        currentState = idleState;
        currentState.EnterState(this);
    }

    public void SwitchState(EnemyState _state)
    {
        //Debug.Log("Came from: " + currentState + " " + Time.time);
        _state.ExitState(this);
        currentState = _state;
        _state.EnterState(this);
        //Debug.Log("Entered: " + currentState + " " + Time.time);
    }

    private void Update()
    {
       
       currentState.FrameUpdate(this);
       debugDestination = agent.destination;
       ShowDebugState();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    public IEnumerator Patrol()
    {
        Debug.Log("patrol started");
        available = false;
        rePositioning = true;

        while (rePositioning)
        {
            Vector3 randomDestination = GetRandomPointAroundPlayer(playerPos.transform.position, circleRadius);
            agent.SetDestination(randomDestination);

            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                transform.LookAt(new Vector3(playerPos.transform.position.x, transform.position.y, playerPos.transform.position.z));
                // Wait until the agent reaches its destination
                yield return null;
            }

            available = true;
            Debug.Log("Reached destination");

            // Wait for a random duration before patrolling again
            yield return new WaitForSeconds(Random.Range(6, 11));

            rePositioning = false;
        }
    }

    void ShowDebugState()
    {
        if (currentState == idleState)
        {
            debugState = DebugState.Idle;
        }

        else if (currentState == chaseState)
        {
            debugState = DebugState.Chase;
        }

        else if (currentState == attackState)
        {
            debugState = DebugState.Attack;
        }

        else if (currentState == circleState)
        {
            debugState = DebugState.Circle;
        }

        else if (currentState == stunnedState)
        {
            debugState = DebugState.Stunned;
        }

        else if (currentState == deadState)
        {
            debugState = DebugState.Dead;
        }

    }

    public bool InAttackRange()
    {
        inAttackRange = Physics.CheckSphere(transform.position, enemy.stats.patrolRange, enemy.whatIsPlayer);
        return inAttackRange;
    }

    Vector3 GetRandomPointAroundPlayer(Vector3 center, float radius)
    {       
        Vector3 randomDirection = Vector3.zero;
        bool foundValidPoint = false;

        while (!foundValidPoint)
        {
            randomDirection = Random.insideUnitSphere * radius;
            randomDirection += center;

            // Check the distance between the random point and the player
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

    public IEnumerator ChasePlayer()
    {
        yield return new WaitForSeconds(2);
        SwitchState(chaseState);
    }

    public void CheckForPlayer()
    {
        Collider[] enemies = Physics.OverlapSphere(enemy.attackPoint.position, enemy.stats.range, enemy.whatIsPlayer);
        if (enemies.Length <= 0) { return; }
        foreach (Collider c in enemies)
        {
            c.GetComponent<IDamageable>().TakeDamage(enemy.stats.damage);
            GameObject hitVFX = Instantiate(enemy.hitVFX, enemy.attackPoint);
            //c.GetComponent<IKnockback>().Knockback(1.5f, orientation);
            //RumbleManager.instance.RumblePulse(.25f, 1f, .25f);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(enemy.attackPoint.position, damageRadius);
    }
}