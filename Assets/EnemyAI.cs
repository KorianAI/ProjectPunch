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
        Stunned,
        Dead
    }

    public State state;

    public EnemyInfo enemy;
    public bool aggro;
    NavMeshAgent agent;

    public bool inAttackRange;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemy.stats.moveSpeed;
    }

    private void Update()
    {
        enemy.anim.SetBool("Walking", state == State.Chase);
        inAttackRange = Physics.CheckSphere(transform.position, enemy.stats.range, enemy.whatIsPlayer);
        HandleStates();
    }

    private void HandleStates()
    {
        switch (state)
        {
            case State.Idle:
                if (aggro) { state = State.Chase; }


                break;

            case State.Chase:
                enemy.Chase(PlayerStateManager.instance.transform);
                if (!aggro) { state = State.Idle; }
                if (inAttackRange) { state = State.Attack; }


                break;

            case State.Attack:
                enemy.Attack(PlayerStateManager.instance.transform);

                if (!inAttackRange) { state = State.Chase; }

                break;

            case State.Stunned:


                break;

            case State.Dead:


                break;
        }
    }
}