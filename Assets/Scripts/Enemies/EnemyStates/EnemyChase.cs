using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : EnemyState
{
    public override void EnterState(EnemyAI enemyAI)
    {
        base.EnterState(enemyAI);
        ai.enemyVisuals.transform.localRotation = Quaternion.Euler(Vector3.zero);
        enemyAI.agent.isStopped = false;
        ai.agent.angularSpeed = 270;
        enemyAI.enemy.anim.SetBool("Walking", true);
        enemyAI.enemy.anim.SetBool("Patrolling", false);
    }

    public override void ExitState(EnemyAI enemyAI)
    {
       base.ExitState(enemyAI);
    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
        base.FrameUpdate(enemyAI);
        if (!enemyAI.InAttackRange())
        {
            ai.agent.SetDestination(ai.transform.position);
            enemyAI.agent.SetDestination(enemyAI.playerPos.transform.position);
        }

        if (enemyAI.InAttackRange())
        {
            enemyAI.SwitchState(new EnemyChaseWait());
            enemyAI.enemy.anim.SetBool("Walking", false);
        }
    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {
        base.PhysicsUpdate(enemyAI);
    }
}
