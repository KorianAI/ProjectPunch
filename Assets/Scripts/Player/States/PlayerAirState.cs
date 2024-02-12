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
        if (player.IsGrounded())
        {
            player.SwitchState(player.moveState);
        }
    }



    public override void PhysicsUpdate(PlayerStateManager player)
    {
        
    }

}
