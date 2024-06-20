using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAirState : PlayerMovementBase
{
    public PlayerStateManager _player;
    bool fallAnim; 

    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _player = player;
        player.pm.readyToJump = false;
    }

    public override void ExitState(PlayerStateManager player)
    {
        
      
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        if (player.pm.grounded && player.pm.yVelocity < 0)
        {
            player.SwitchState(player.moveState);
        }

 
    }


    public override void PhysicsUpdate(PlayerStateManager player)
    {

           
     


        player.pm.velocity.y = player.pm.yVelocity;
        player.pm.controller.Move(player.pm.velocity * Time.deltaTime);
       
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        base.HandleBufferedInput(command);
    }

}