using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretLocked : TurretState
{
    public override void EnterState(TurretAI turretAI)
    {
        base.EnterState(turretAI);
        ai = turretAI;
        turretAI.locked = false;
        turretAI.fired = false;

        if (!turretAI.InAttackRange()) //checks that turret is still able to lock on
        {
            turretAI.SwitchState(new TurretIdle());
        }
        else
        {
            turretAI.turretHead.transform.DOKill(false); //stop previous head movement
            turretAI.turretHead.transform.DOLookAt(turretAI.playerPos.transform.position, turretAI.lockToPlayerDuration).SetEase(Ease.Linear).OnComplete(CompletedLock); //look at player
        }
    }

    void CompletedLock()
    {
        ai.locked = true;
        ai.StartCoroutine(ai.FireCountdown()); //start countdown until firing
    }

    public override void ExitState(TurretAI turretAI)
    {
        base.ExitState(turretAI);
        turretAI.locked = false;
    }

    public override void FrameUpdate(TurretAI turretAI)
    {
        base.FrameUpdate(turretAI);

        if (turretAI.locked) //keeps turret focused on player
        {
            //turretAI.turretHead.transform.LookAt(new Vector3(turretAI.playerPos.transform.position.x, turretAI.playerPos.transform.position.y, turretAI.playerPos.transform.position.z)); //look at player pos
            turretAI.turretHead.transform.LookAt(turretAI.smoothedLinePosition); //look at player pos, smoothed
        }

        if (!turretAI.InAttackRange()) //returns to idle state if out of range
        {
            turretAI.locked = false;
            turretAI.SwitchState(new TurretIdle());
        }
    }

    public override void PhysicsUpdate(TurretAI turretAI)
    {
        base.PhysicsUpdate(turretAI);
    }
}
