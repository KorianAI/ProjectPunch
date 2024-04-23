using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAttack : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.transform.DOMove(player.transform.position + (player.orientation.transform.forward / 2), .5f);
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        if (!player.grounded)
        {
            player.transform.DOMove(player.transform.position + (player.orientation.transform.forward), .5f);
            player.SwitchState(player.inAirState);
            player.canAttack = true;
            player.cam.canRotate = true;
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {

    }
}
