using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
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
    public EnemyDead dead = new EnemyDead();

    [Header("References")]
    public CombatManager manager;
    public EnemyInfo enemy;
    public NavMeshAgent agent;
    public GameObject playerPos;

    [Header("Conditions")]
    public bool aggro;
    public bool inAttackRange;
    public bool permissionToAttack;
    public bool patrolling;

    public float circleRadius;
    public float circleSpeed;

    public Coroutine patrol;

    public float remainingDistance;



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
        Debug.Log("Came from: " + currentState + " " + Time.time);
        _state.ExitState(this);
        currentState = _state;
        _state.EnterState(this);
        Debug.Log("Entered: " + currentState + " " + Time.time);
    }

    private void Update()
    {
       
       currentState.FrameUpdate(this);
       
        

       ShowDebugState();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    public IEnumerator Patrol()
    {
        patrolling = true;
        manager.MakeAgentsCircleTarget(this);
        remainingDistance = agent.remainingDistance;
        enemy.anim.SetBool("Walking", true);
        yield return new WaitUntil(() => remainingDistance < agent.stoppingDistance);
        enemy.anim.SetBool("Walking", false);
        Debug.Log("dude");
        yield return new WaitForSeconds(Random.Range(6, 9));
        patrolling = false;
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

        else if (currentState == dead)
        {
            debugState = DebugState.Dead;
        }

    }

    public bool InAttackRange()
    {
        inAttackRange = Physics.CheckSphere(transform.position, enemy.stats.range, enemy.whatIsPlayer);
        return inAttackRange;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, enemy.stats.range);
    }
}