using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.readyToJump = false;

        player.rb.velocity = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);

        player.rb.AddForce(player.transform.up * player.jumpForce, ForceMode.Impulse);
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        if (player.grounded && player.rb.velocity.y < 0f)
        {
            player.SwitchState(player.idleState);
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        if (player.rb.velocity.y < 0f)
        {
            player.rb.velocity += Vector3.up * Physics.gravity.y * 5f * Time.deltaTime;
        }
    }

}
