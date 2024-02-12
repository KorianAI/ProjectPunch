using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveState : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.animHandler.ChangeAnimationState("PlayerWalk");
    }

    public override void ExitState(PlayerStateManager player)
    {

    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        // ground check
        // player.grounded = Physics.Raycast(player.transform.position, Vector3.down, player.playerHeight * 0.5f + 0.3f, player.whatIsGround);

        MovementInput(player);
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

    private void MovementInput(PlayerStateManager player)
    {
        Vector2 movementInput = player.move.action.ReadValue<Vector2>();

        player.horizontalInput = movementInput.x;
        player.verticalInput = movementInput.y;
    }

    private void MovePlayer(PlayerStateManager player)
    {
        // calculate movement direction
        player.moveDirection = player.orientation.forward * player.verticalInput + player.orientation.right * player.horizontalInput;

        // on ground
        if (player.IsGrounded())
            //player.rb.AddForce(player.moveDirection.normalized * player.moveSpeed * 10f, ForceMode.Force);
            player.controller.SimpleMove(player.moveDirection * player.moveSpeed);

        // in air
        else if (!player.IsGrounded())
            //player.rb.AddForce(player.moveDirection.normalized * player.moveSpeed * 10f * player.airMultiplier, ForceMode.Force);
            player.controller.SimpleMove(player.moveDirection * player.moveSpeed);
    }
}
