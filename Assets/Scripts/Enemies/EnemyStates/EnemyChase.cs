using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : EnemyState
{
    public override void EnterState(EnemyAI enemyAI)
    {
        enemyAI.agent.isStopped = false;
    }

    public override void ExitState(EnemyAI enemyAI)
    {

    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
        if (!enemyAI.InAttackRange())
        {
            enemyAI.agent.SetDestination(enemyAI.playerPos.transform.position);
        }

        if (enemyAI.InAttackRange())
        {
            enemyAI.SwitchState(enemyAI.attackState);
        }
    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {

    }
}
