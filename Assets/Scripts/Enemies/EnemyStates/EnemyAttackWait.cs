using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackWait : EnemyState
{
    public override void EnterState(EnemyAI enemyAI)
    {
        base.EnterState(enemyAI);
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
            enemyAI.SwitchState(new EnemyChase());
        }
        // if not in combat range > swap to chase state
    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {
        base.ExitState(enemyAI);
    }
}
