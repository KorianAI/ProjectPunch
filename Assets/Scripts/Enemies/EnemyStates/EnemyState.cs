using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public virtual void EnterState(EnemyAI enemyAI)
    {

    }
    public virtual void ExitState(EnemyAI enemyAI)
    {

    }
    public virtual void FrameUpdate(EnemyAI enemyAI)
    {

    } // regular update
    public virtual void PhysicsUpdate(EnemyAI enemyAI)
    {

    } // fixed update
}
