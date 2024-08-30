using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackWait : EnemyState
{
    float chaseWaitPeriod = .2f;
    float timer;
    public bool chasing;

    public override void EnterState(EnemyAI enemyAI)
    {
        base.EnterState(enemyAI);
        enemyAI.enemy.anim.SetBool("Patrolling", false);
        ai.agent.SetDestination(ai.transform.position);
        ai.agent.isStopped = true;
    }

    public override void ExitState(EnemyAI enemyAI)
    {
        base.ExitState(enemyAI);
    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
        base.FrameUpdate(enemyAI);

        ai.transform.LookAt(new Vector3(ai.playerPos.transform.position.x, ai.transform.position.y, ai.playerPos.transform.position.z));

        if (enemyAI.attackToken)
        {
            enemyAI.SwitchState(new EnemyAttack());
        }

        if (!enemyAI.InAttackRange())
        {
            if (!chasing)
            {
                chasing = true;
                timer = chaseWaitPeriod;
            }    
        }

        else
        {
            chasing = false;
            timer = chaseWaitPeriod;
        }

        if (timer > 0 && chasing)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                enemyAI.SwitchState(new EnemyChase());
            }
        }

        // if not in combat range > swap to chase state

        // leave range > set timer to 2 & a bool to true, if timer > 0, count down, if timer counts down past 0 > set to enemy chase
    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {
        base.ExitState(enemyAI);
    }
}
