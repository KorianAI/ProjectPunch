using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIdle : TurretState
{
    public override void EnterState(TurretAI turretAI)
    {
        base.EnterState(turretAI);
        turretAI.UpdateLookPosition();
    }

    public override void ExitState(TurretAI turretAI)
    {
        base.ExitState(turretAI);
    }

    public override void FrameUpdate(TurretAI turretAI)
    {
        base.FrameUpdate(turretAI);
        if (turretAI.InAttackRange() || Input.GetKeyDown(KeyCode.Alpha8))
        {
            turretAI.SwitchState(new TurretLocked());
            Debug.Log("Enemy in range of '" + turretAI.gameObject.name + "' turret: ");
        }
    }

    public override void PhysicsUpdate(TurretAI turretAI)
    {
        base.PhysicsUpdate(turretAI);
    }
}
