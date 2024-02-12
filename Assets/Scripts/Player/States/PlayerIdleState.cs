using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerIdleState : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.animHandler.ChangeAnimationState("PlayerIdle");
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {

        if (!player.IsGrounded())
        {
            player.SwitchState(player.inAirState);
        }

        if (player.move.action.ReadValue<Vector2>() != Vector2.zero)
        {
            player.SwitchState(player.moveState);
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        
    }
}
