using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpStart : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _sm.cam.canRotate = false;
        _sm.ih.SetCanConsumeInput(false);
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        base.FrameUpdate(player);
    }


    public override void HandleBufferedInput(InputCommand command)
    {
        base.HandleBufferedInput(command);
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

}
