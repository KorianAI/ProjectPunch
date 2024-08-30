using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyState
{
    public override void EnterState(EnemyAI enemyAI)
    {
        base.EnterState(enemyAI);
        enemyAI.agent.isStopped = true;
    }

    public override void ExitState(EnemyAI enemyAI)
    {
      base.ExitState(enemyAI);
    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
        base.FrameUpdate(enemyAI);
        if (enemyAI.aggro)
        {
            if (enemyAI.InAttackRange())
            {
                enemyAI.SwitchState(new EnemyChaseWait());
            }

            else
            {
                enemyAI.SwitchState(new EnemyChase()); 
            }
        }
    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {
        base.PhysicsUpdate(enemyAI);
    }
}
