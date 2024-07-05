using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAirState : PlayerMovementBase
{
    public PlayerStateManager _player;
    bool fallAnim;

    float atkcooldown = .25f;

    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _sm.ih.SetCanConsumeInput(false);
        _sm.anim.SetBool("AirAttack", false);
        _sm.anim.SetBool("isGrounded", false);
        _player = player;
        player.pm.readyToJump = false;
        _sm.cam.canRotate = false;
    }

    public override void ExitState(PlayerStateManager player)
    {
        
      
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        base.FrameUpdate(player);
        _sm.pm.ApplyGravity(2);
        Debug.Log(fixedtime);
        //Debug.Log(time);
        if (fixedtime > atkcooldown)
        {
            if (_sm.ih.GetBufferedInputs().Length > 0)
            {
                _sm.ih.SetCanConsumeInput(true);
            }
        }


        if (player.pm.grounded && player.pm.yVelocity < 0)
        {
            player.SwitchState(player.moveState);
        }

 
    }


    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
        player.pm.velocity.y = player.pm.yVelocity;
        player.pm.controller.Move(player.pm.velocity * Time.deltaTime);
       
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        if (fixedtime > atkcooldown)
        {
            base.HandleBufferedInput(command);
        }

    }

}
