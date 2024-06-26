using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEntry : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {

        if (_sm.pm.grounded)
        {
            // enter ground combo
        }

        else
        {
            _sm.SwitchState(new PlayerAirState());
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
