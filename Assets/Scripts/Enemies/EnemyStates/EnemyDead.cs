using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : EnemyState
{
    public override void EnterState(EnemyAI enemyAI)
    {
        enemyAI.agent.isStopped = true;
        enemyAI.GetComponent<Collider>().enabled = false;
        enemyAI.enemy.anim.SetTrigger("Dies");
        enemyAI.rePositioning = false;
        enemyAI.available = false;
    }

    public override void ExitState(EnemyAI enemyAI)
    {

    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {

    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {

    }
}
