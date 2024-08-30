using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirborne : EnemyState
{
    float gravTimer = 1.5f;
    public override void EnterState(EnemyAI enemyAI)
    {
        base.EnterState(enemyAI);
        ai.inAir = true;
        enemyAI.agent.enabled = false;
        enemyAI.rb.useGravity = false;
    }

    public override void ExitState(EnemyAI enemyAI)
    {
        base.ExitState(enemyAI);
    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
        base.FrameUpdate(enemyAI);

        if (time >= gravTimer)
        {
            enemyAI.rb.useGravity = true;

            if (!ai.inAir)
            {
                enemyAI.rb.useGravity = false;
                ai.SwitchState(new EnemyIdle());
            }
        }
    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {
        base.PhysicsUpdate(enemyAI);
    }
}
