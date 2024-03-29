using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyState
{
    public override void EnterState(EnemyAI enemyAI)
    {
        enemyAI.agent.isStopped = true;
    }

    public override void ExitState(EnemyAI enemyAI)
    {
      
    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
        if (enemyAI.aggro && !enemyAI.InAttackRange())
        {
            enemyAI.SwitchState(enemyAI.chaseState);
        }

        else if (enemyAI.aggro && enemyAI.InAttackRange())
        {
            enemyAI.SwitchState(enemyAI.attackState);
        }
    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {
        
    }
}
