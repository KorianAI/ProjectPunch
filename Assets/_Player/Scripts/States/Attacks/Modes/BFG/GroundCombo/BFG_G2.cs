using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG_G2 : PlayerGroundAttack
{
    public override void EnterState(PlayerStateManager player)
    {
        atkMoveDistance = 2f;
        atkMoveDur = .4f;
        duration = .3f;
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
            canAttack = true;
            attackIndex = 2;
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
        if (canAttack)
        {
            base.HandleBufferedInput(command);
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

}
