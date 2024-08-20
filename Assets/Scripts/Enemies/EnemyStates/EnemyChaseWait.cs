using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseWait : EnemyState
{

    public override void EnterState(EnemyAI enemyAI)
    {
        base.EnterState(enemyAI);
        ai.agent.SetDestination(ai.transform.position);
        ai.transform.LookAt(new Vector3(ai.playerPos.transform.position.x, ai.transform.position.y, ai.playerPos.transform.position.z));
    }

    public override void ExitState(EnemyAI enemyAI)
    {
        base.ExitState(enemyAI);
    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
        base.FrameUpdate(enemyAI);
        if (enemyAI.circleToken)
        {
            enemyAI.SwitchState(new EnemyCircle());
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
