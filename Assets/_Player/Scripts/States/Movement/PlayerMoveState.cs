using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveState : PlayerMovementBase
{
    
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        player.anim.SetBool("isWalking", true);
        _sm = player;
        _sm.pc.ResetAirGrav();
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.anim.SetBool("isWalking", false);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        _sm.pm.ApplyGravity(1);

        player.pm.MovementInput();

        if (InputMapManager.inputActions.Player.Movement.ReadValue<Vector2>() == Vector2.zero)
        {
            player.SwitchState(player.idleState);
        }


    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        MovePlayer(player);
    }

    private void MovePlayer(PlayerStateManager player)
    {
        // calculate movement enemy
        player.pm.moveDirection = player.orientation.forward * player.pm.verticalInput + player.orientation.right * player.pm.horizontalInput;

        // on ground
        player.pm.controller.SimpleMove(player.pm.velocity);
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        base.HandleBufferedInput(command);
    }
}
