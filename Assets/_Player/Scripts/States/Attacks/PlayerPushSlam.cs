using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushSlam : PlayerAttackBase
{
    bool canFall = false;

    public override void EnterState(PlayerStateManager player)
    {
        player.transform.DOKill();
        duration = .2f;
        base.EnterState(player);
        if (!_sm.pm.grounded) { _sm.anim.SetBool("AirAttack", true); }
        _sm.anim.Play("PushSlam");
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {

    }

    public override void HandleBufferedInput(InputCommand command)
    {

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
        //if (canFall)
        //{
        //    player.pm.velocity.y = player.pm.yVelocity;
        //    player.pm.controller.Move(player.pm.velocity * Time.deltaTime);
        //}
    }
}
