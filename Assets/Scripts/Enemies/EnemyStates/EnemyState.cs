using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    public abstract void EnterState(EnemyAI enemyAI);
    public abstract void ExitState(EnemyAI enemyAI);
    public abstract void FrameUpdate(EnemyAI enemyAI); // regular update
    public abstract void PhysicsUpdate(EnemyAI enemyAI); // fixed update
}
