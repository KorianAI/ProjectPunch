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

        player.rb.velocity = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);

        player.rb.AddForce(player.transform.up * player.jumpForce, ForceMode.Impulse);

        player.animHandler.ChangeAnimationState("PlayerInAir");

        player.playerHeight /= 2;
        player.GetComponent<CapsuleCollider>().height = 1.6f;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.playerHeight *= 2;
        player.GetComponent<CapsuleCollider>().height = 2.36f;
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        if (player.grounded && player.rb.velocity.y < 0f)
        {
            player.LandAnim();
        }

    }



    public override void PhysicsUpdate(PlayerStateManager player)
    {
        player.moveDirection = player.orientation.forward * player.verticalInput + player.orientation.right * player.horizontalInput;

        if (player.rb.velocity.y < 0f)
        {
            player.rb.velocity += Vector3.up * Physics.gravity.y * 5f * Time.deltaTime;
        }

        player.rb.AddForce(player.moveDirection.normalized * player.moveSpeed * 10f * player.airMultiplier, ForceMode.Force);
    }

}
