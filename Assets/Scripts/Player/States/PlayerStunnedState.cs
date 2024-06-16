using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunnedState : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.velocity = Vector3.zero;
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        player.velocity.y = player.yVelocity;
        player.controller.Move(player.velocity * Time.deltaTime);
    }

    public override void HandleBufferedInput(InputCommand command)
    {

    }
}
