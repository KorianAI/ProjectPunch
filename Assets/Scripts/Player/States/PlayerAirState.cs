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

        player.animHandler.ChangeAnimationState("PlayerInAir");

        //player.playerHeight /= 2;
      

    }

    public override void ExitState(PlayerStateManager player)
    {
        //player.playerHeight *= 2;
      
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        player.ApplyGravity();
        player.MovementDirection();
        
        player.controller.Move(player.moveDirection * player.moveSpeed * Time.deltaTime);
    }



    public override void PhysicsUpdate(PlayerStateManager player)
    {
        
    }

}
