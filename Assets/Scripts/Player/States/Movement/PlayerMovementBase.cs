using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBase : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _sm.inputHandler.SetCanConsumeInput(true);
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
        if (command == null) return;

        if (_sm.resources.shift != null)
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

            else if (command.Type == InputType.A)
            {
                _sm.pm.Jump();
            }
        }

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }
}
