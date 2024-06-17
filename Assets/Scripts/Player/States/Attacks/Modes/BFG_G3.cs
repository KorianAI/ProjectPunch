using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG_G3 : PlayerAttackBase
{
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        duration = 1f;
        player.anim.SetTrigger("HeavyAttack3");
        canAttack = false;
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        base.FrameUpdate(player);
        if (fixedtime > duration)
        {
            if (_sm.inputHandler.GetBufferedInputs().Length > 0)
            {
                _sm.inputHandler.SetCanConsumeInput(true);
            }

            else
            {
                _sm.SwitchState(new PlayerIdleState());
            }
        }
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        if (command.Type == InputType.X)
        {
            _sm.resources.attachment.Input(command, _sm.pm.grounded);
        }

        else
        {
            _sm.SwitchState(new PlayerIdleState());
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }
}
