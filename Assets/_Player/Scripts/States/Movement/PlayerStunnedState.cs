using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunnedState : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.pm.velocity = Vector3.zero;
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        player.pm.velocity.y = player.pm.yVelocity;
        player.controller.Move(player.pm.velocity * Time.deltaTime);
    }

    public override void HandleBufferedInput(InputCommand command)
    {

    }
}
