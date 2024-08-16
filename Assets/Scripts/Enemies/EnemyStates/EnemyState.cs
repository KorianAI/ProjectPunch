using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public EnemyAI ai;
    protected float time { get; set; }
    protected float fixedtime { get; set; }
    public virtual void EnterState(EnemyAI enemyAI)
    {
        ai = enemyAI;
        ai.enemyVisuals.transform.localRotation = Quaternion.Euler(new Vector3(0, 25, 0));
      // Debug.Log("Entering" + this.GetType().Name);
    }
    public virtual void ExitState(EnemyAI enemyAI)
    {
       
       //Debug.Log("Leaving" + this.GetType().Name);
    }
    public virtual void FrameUpdate(EnemyAI enemyAI)
    {
        time += Time.deltaTime;

    } // regular update
    public virtual void PhysicsUpdate(EnemyAI enemyAI)
    {
        time += Time.deltaTime;
    } // fixed update
}
