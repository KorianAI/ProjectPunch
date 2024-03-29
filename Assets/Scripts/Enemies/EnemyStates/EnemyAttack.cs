using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    public override void EnterState(EnemyAI enemyAI)
    {

    }

    public override void ExitState(EnemyAI enemyAI)
    {

    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
        enemyAI.enemy.Attack(enemyAI.playerPos.transform);
    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {

    }
}