using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircle : EnemyState
{

    public override void EnterState(EnemyAI enemyAI)
    {
        //enemyAI.manager.MakeAgentsCircleTarget(enemyAI);
    }

    public override void ExitState(EnemyAI enemyAI)
    {

    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
        if (enemyAI.permissionToAttack)
        {
            enemyAI.SwitchState(enemyAI.attackState);
        }
    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {

    }
}
