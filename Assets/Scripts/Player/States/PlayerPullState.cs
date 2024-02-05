using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerPullState : PlayerState
{
    PlayerStateManager _p;

    public override void EnterState(PlayerStateManager player)
    {

        _p = player;
        player.canAttack = false;
        GameObject target = player.lockOn.currentTarget.gameObject;

        if (target != null)
        {
            target.transform.DOMove(player.pullPosition.position, .5f).onComplete = PullFinished;
        }
    }

    void PullFinished()
    {
        _p.canAttack = true;
        _p.SwitchState(_p.idleState);
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        
    }
}
