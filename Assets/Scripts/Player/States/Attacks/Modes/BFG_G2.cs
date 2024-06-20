using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG_G2 : PlayerAttackBase
{
    public override void EnterState(PlayerStateManager player)
    {
        atkMoveDistance = 2f;
        atkMoveDur = .4f;
        duration = .4f;
        base.EnterState(player);

        player.anim.SetTrigger("HeavyAttack2");
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
            if (_sm.ih.GetBufferedInputs().Length > 0)
            {
                _sm.ih.SetCanConsumeInput(true);
            }

            else
            {
                if (fixedtime > animator.GetCurrentAnimatorStateInfo(0).length)
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

        else if (command.Type == InputType.Y)
        {
            Debug.Log("Heavy Attack received in heavy state");
            _sm.SwitchState(new BFG_G3());
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

}
