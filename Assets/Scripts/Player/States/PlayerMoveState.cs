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
        player.grounded = Physics.Raycast(player.transform.position, Vector3.down, player.playerHeight * 0.5f + 0.3f, player.whatIsGround);

        MovementInput(player);
        SpeedControl(player);

        if (player.move.action.ReadValue<Vector2>() == Vector2.zero)
        {
            player.SwitchState(player.idleState);
        }

        // handle drag
        if (player.grounded)
            player.rb.drag = player.groundDrag;
        else
            player.rb.drag = 0;


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

        //if (movementInput == Vector2.zero)
        //{
        //    animHandler.ChangeAnimationState(idleAnim);
        //}

        //else
        //{
        //    animHandler.ChangeAnimationState(walkAnim);
        //}
    }

    private void MovePlayer(PlayerStateManager player)
    {
        // calculate movement direction
        player.moveDirection = player.orientation.forward * player.verticalInput + player.orientation.right * player.horizontalInput;

        // on ground
        if (player.grounded)
            player.rb.AddForce(player.moveDirection.normalized * player.moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!player.grounded)
            player.rb.AddForce(player.moveDirection.normalized * player.moveSpeed * 10f * player.airMultiplier, ForceMode.Force);
    }

    private void SpeedControl(PlayerStateManager player)
    {
        Vector3 flatVel = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > player.moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * player.moveSpeed;
            player.rb.velocity = new Vector3(limitedVel.x, player.rb.velocity.y, limitedVel.z);
        }
    }
}
