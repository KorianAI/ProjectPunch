using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBase : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _sm.ih.SetCanConsumeInput(true);
        if (_sm.pm.grounded)
        {
            _sm.cam.canRotate = true;
        }
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