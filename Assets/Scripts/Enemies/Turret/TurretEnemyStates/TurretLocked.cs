using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretLocked : TurretState
{
    bool locked;

    public override void EnterState(TurretAI turretAI)
    {
        base.EnterState(turretAI);
        turretAI.turretHead.transform.DOKill(false); //stop head movement

        locked = false;
        turretAI.turretHead.transform.DOLookAt(turretAI.playerPos.transform.position, .5f).SetEase(Ease.Linear).OnComplete(CompletedLock);
        //play charging anims
        //start countdown
        turretAI.StartCoroutine(turretAI.FireCountdown());
    }

    void CompletedLock()
    {
        locked = true;
    }

    public override void ExitState(TurretAI turretAI)
    {
        base.ExitState(turretAI);
    }

    public override void FrameUpdate(TurretAI turretAI)
    {
        base.FrameUpdate(turretAI);

        if (locked)
        {
            turretAI.turretHead.transform.LookAt(new Vector3(turretAI.playerPos.transform.position.x, turretAI.playerPos.transform.position.y, turretAI.playerPos.transform.position.z)); //look at player pos
        }
    }

    public override void PhysicsUpdate(TurretAI turretAI)
    {
        base.PhysicsUpdate(turretAI);
    }
}
