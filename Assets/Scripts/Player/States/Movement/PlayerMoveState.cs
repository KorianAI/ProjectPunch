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
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.anim.SetBool("isWalking", false);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        // ground check
        // collision.grounded = Physics.Raycast(collision.transform.position, Vector3.down, collision.playerHeight * 0.5f + 0.3f, collision.whatIsGround);

        player.pm.MovementInput();
        //SpeedControl(collision);

        if (player.ih.InputMaster.Player.Movement.ReadValue<Vector2>() == Vector2.zero)
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
        // calculate movement direction
        player.pm.moveDirection = player.orientation.forward * player.pm.verticalInput + player.orientation.right * player.pm.horizontalInput;

        // on ground
        player.pm.controller.SimpleMove(player.pm.velocity);
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        base.HandleBufferedInput(command);
    }
}
