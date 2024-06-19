using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG_G1 : PlayerAttackBase
{
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        duration = .5f;
        player.anim.SetTrigger("HeavyAttack1");
        canAttack = false;
        base.MoveForward(_sm, 2f, .2f);
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        base.FrameUpdate(player);
        if (fixedtime > duration)
        {
            canAttack = true;

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

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        if (canAttack)
        {
            if (command.Type == InputType.X)
            {
                _sm.resources.attachment.Input(command, _sm.pm.grounded);
            }

            else if (command.Type == InputType.Y)
            {
                Debug.Log("Heavy Attack received in heavy state");
                _sm.SwitchState(new BFG_G2());
            }

            else
            {
                base.HandleBufferedInput(command);
            }
        }
    }
}
