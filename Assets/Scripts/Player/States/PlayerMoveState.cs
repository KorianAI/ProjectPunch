using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveState : PlayerState
{

    public override void EnterState(PlayerStateManager player)
    {
        player.anim.SetBool("isWalking", true);
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.anim.SetBool("isWalking", false);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        // ground check
        // player.grounded = Physics.Raycast(player.transform.position, Vector3.down, player.playerHeight * 0.5f + 0.3f, player.whatIsGround);

        player.MovementInput();
        //SpeedControl(player);

        if (player.move.action.ReadValue<Vector2>() == Vector2.zero)
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
        player.moveDirection = player.orientation.forward * player.verticalInput + player.orientation.right * player.horizontalInput;

        // on ground
        player.controller.SimpleMove(player.velocity);
    }
}
