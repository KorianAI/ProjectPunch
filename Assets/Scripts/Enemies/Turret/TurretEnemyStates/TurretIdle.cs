using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIdle : TurretState
{
    public override void EnterState(TurretAI turretAI)
    {
        base.EnterState(turretAI);
        turretAI.locked = false;
        turretAI.UpdateIdleLookPosition();
        turretAI.StopCoroutine(turretAI.FireCountdown());
    }

    public override void ExitState(TurretAI turretAI)
    {
        base.ExitState(turretAI);
    }

    public override void FrameUpdate(TurretAI turretAI)
    {
        base.FrameUpdate(turretAI);
        if (turretAI.InAttackRange())
        {
            turretAI.SwitchState(new TurretLocked());
        }
    }

    public override void PhysicsUpdate(TurretAI turretAI)
    {
        base.PhysicsUpdate(turretAI);
    }
}
