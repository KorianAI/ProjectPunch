using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Circle,
        Stunned,
        Dead
    }

    public State state;

    public EnemyInfo enemy;
    public bool aggro;
    public NavMeshAgent agent;

    public bool inAttackRange;
    public GameObject playerPos;

    public bool permissionToAttack;

    public CombatManager manager;



    private void Start()
    {
        playerPos = PlayerStateManager.instance.gameObject;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(enemy.stats.moveSpeed - 1, enemy.stats.moveSpeed);
        enemy.ai = this;
    }

    private void Update()
    {
        enemy.anim.SetBool("Walking", state == State.Chase);
        inAttackRange = Physics.CheckSphere(transform.position, enemy.stats.range, enemy.whatIsPlayer);
        StateUpdate();
    }

    private void FixedUpdate()
    {
        StateFixedUpdate();
    }

    private void StateUpdate()
    {
        switch (state)
        {
            case State.Idle:
                if (aggro && !inAttackRange) { state = State.Chase; }
                if (inAttackRange && permissionToAttack) { state = State.Attack; }
                break;

            case State.Chase:

                if (!aggro) { state = State.Idle; }
                if (inAttackRange && permissionToAttack) { state = State.Attack; }


                break;

            case State.Attack:

                if (!inAttackRange) { state = State.Chase; }

                break;

            case State.Stunned:

                enemy.Stunned(playerPos.transform);
                break;

            case State.Dead:


                break;
            case State.Circle:
                if (inAttackRange && permissionToAttack) { state = State.Attack; }
                break;
        }
    }

    private void StateFixedUpdate()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Chase:
                enemy.Chase(playerPos.transform);
                break;
            case State.Attack:
                enemy.Attack(playerPos.transform);
                break;
            case State.Stunned:
                break;
            case State.Dead:
                break;
        }
    }
}