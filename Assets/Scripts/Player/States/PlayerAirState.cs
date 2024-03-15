using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerStateManager _player;
    bool fallAnim;

    public override void EnterState(PlayerStateManager player)
    {
        
        _player = player;
        player.readyToJump = false;     
    }

    public override void ExitState(PlayerStateManager player)
    {
        
      
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        if (player.grounded && player.yVelocity < 0)
        {
            player.SwitchState(player.moveState);
        }

        player.MovementInput();
    }



    public override void PhysicsUpdate(PlayerStateManager player)
    {
        player.moveDirection = player.orientation.forward * player.verticalInput + player.orientation.right * player.horizontalInput;

        player.velocity.y = player.yVelocity;
        player.controller.Move(player.velocity * Time.deltaTime);
    }

}
