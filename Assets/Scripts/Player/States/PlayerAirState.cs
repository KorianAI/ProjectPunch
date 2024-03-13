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
        player.velocity.x = 0;
        player.velocity.z = 0;
    }

    public override void ExitState(PlayerStateManager player)
    {
        
      
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        if (player.IsGrounded() && player.yVelocity < 0)
        {
            player.SwitchState(player.moveState);
        }

        
    }



    public override void PhysicsUpdate(PlayerStateManager player)
    {
        player.velocity.y = player.yVelocity;
        player.controller.Move(player.velocity * Time.deltaTime);
    }

}
