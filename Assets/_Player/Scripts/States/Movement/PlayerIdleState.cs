using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerIdleState : PlayerMovementBase
{
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        
        player.pm.velocity = Vector3.zero;
        player.pm.moveDirection = Vector3.zero;
       
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        _sm.pm.ApplyGravity(1);


        if (!player.pm.grounded)
        {
            player.SwitchState(new PlayerAirState());
        }

        if (player.ih.InputMaster.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
        {
            player.SwitchState(player.moveState);
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        base.HandleBufferedInput(command);
    }
}
