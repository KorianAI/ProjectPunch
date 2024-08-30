using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretLocked : TurretState
{
    bool locked;
    TurretAI ai;

    public override void EnterState(TurretAI turretAI)
    {
        base.EnterState(turretAI);
        ai = turretAI;
        locked = false;

        turretAI.turretHead.transform.DOKill(false); //stop previous head movement
        turretAI.turretHead.transform.DOLookAt(turretAI.playerPos.transform.position, .5f).SetEase(Ease.Linear).OnComplete(CompletedLock); //move quickly to player
    }

    void CompletedLock()
    {
        locked = true;
        //play charging anims

        ai.StartCoroutine(ai.FireCountdown()); //start countdown until firing
    }

    public override void ExitState(TurretAI turretAI)
    {
        base.ExitState(turretAI);
    }

    public override void FrameUpdate(TurretAI turretAI)
    {
        base.FrameUpdate(turretAI);

        if (locked) //keeps turret focused on player
        {
            turretAI.turretHead.transform.LookAt(new Vector3(turretAI.playerPos.transform.position.x, turretAI.playerPos.transform.position.y, turretAI.playerPos.transform.position.z)); //look at player pos
        }

        if (!turretAI.InAttackRange()) //returns to idle state if out of range
        {
            turretAI.SwitchState(new TurretIdle());
        }
    }

    public override void PhysicsUpdate(TurretAI turretAI)
    {
        base.PhysicsUpdate(turretAI);
    }
}
