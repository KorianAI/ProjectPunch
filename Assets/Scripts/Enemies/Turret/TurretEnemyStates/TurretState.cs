using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretState
{
    public TurretAI ai;
    protected float time { get; set; }
    protected float fixedtime { get; set; }
    public virtual void EnterState(TurretAI turretAI)
    {
        ai = turretAI;
        //ai.enemyVisuals.transform.localRotation = Quaternion.Euler(new Vector3(0, 25, 0));
        Debug.Log("Entering" + this.GetType().Name);
    }
    public virtual void ExitState(TurretAI turretAI)
    {
        Debug.Log("Leaving" + this.GetType().Name);
    }
    public virtual void FrameUpdate(TurretAI turretAI) // regular update
    {
        time += Time.deltaTime;
    } 
    public virtual void PhysicsUpdate(TurretAI turretAI) // fixed update
    {
        time += Time.deltaTime;
    } 
}
