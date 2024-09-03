using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDead : TurretState
{
    public override void EnterState(TurretAI turretAI)
    {
        turretAI.dead = true;
        turretAI.GetComponent<Collider>().enabled = false;
        turretAI.locked = false;
        turretAI.StopCoroutine(turretAI.FireCountdown());
    }

    public override void ExitState(TurretAI turretAI)
    {

    }

    public override void FrameUpdate(TurretAI turretAI)
    {

    }

    public override void PhysicsUpdate(TurretAI turretAI)
    {

    }
}
