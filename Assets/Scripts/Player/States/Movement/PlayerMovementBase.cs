using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBase : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
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
        if (_sm.resources.scrapShift)
        {
            _sm.SwitchState(new PlayerIdleState());
        }

        else
        {
            if (command.Type == InputType.X)
            {
                Debug.Log("X received in move state");
                _sm.resources.attachment.Input(command, _sm.pm.grounded);
            }

            else if (command.Type == InputType.Y)
            {
                Debug.Log("Heavy Attack received in move state");
                _sm.resources.mode.Input(command, _sm.pm.grounded);
            }
        }

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }
}
