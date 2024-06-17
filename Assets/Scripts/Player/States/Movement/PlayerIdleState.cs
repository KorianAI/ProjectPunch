using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerIdleState : PlayerMovementBase
{
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _sm = player;
        player.pm.velocity = Vector3.zero;
        _sm.inputHandler.SetCanConsumeInput(true);
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {

        if (!player.pm.grounded)
        {
            player.SwitchState(player.inAirState);
        }

        if (player.inputHandler.InputMaster.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
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
