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
        if (command == null) return;

        if (_sm.resources.shift != null)
        {
            _sm.SwitchState(new PlayerIdleState());
        }

        else
        {
            if (command.Type == InputType.X)
            {
                
                _sm.resources.attachment.WeaponInput(command, _sm.pm.grounded, 0);
            }

            else if (command.Type == InputType.Y)
            {
                
                _sm.resources.mode.WeaponInput(command, _sm.pm.grounded, 0);
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
