using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFiring : TurretState
{
    public override void EnterState(TurretAI turretAI)
    {
        base.EnterState(turretAI);
        //fire projectile at high speed
        Debug.Log(turretAI.gameObject.name + "Fired!");
        turretAI.SwitchState(new TurretLocked());
    }

    public override void ExitState(TurretAI turretAI)
    {
        base.ExitState(turretAI);
    }

    public override void FrameUpdate(TurretAI turretAI)
    {
        base.FrameUpdate(turretAI);
    }

    public override void PhysicsUpdate(TurretAI turretAI)
    {
        base.PhysicsUpdate(turretAI);
    }
}
