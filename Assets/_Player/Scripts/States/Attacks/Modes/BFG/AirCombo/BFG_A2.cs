using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG_A2 : PlayerAirAttack
{
    public override void EnterState(PlayerStateManager player)
    {
        rangeAttack = false;
        atkMoveDistance = 2.25f;
        atkMoveDur = .4f;
        duration = .3f;
        base.EnterState(player);
        player.anim.Play("HeavyAttack2");
        canAttack = false;
        _sm.pc.SaveAtkIndex(2);
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
            canFall = true;

            if (_sm.ih.GetBufferedInputs().Length > 0)
            {
                _sm.ih.SetCanConsumeInput(true);
            }

            else
            {
                if (fixedtime > animator.GetCurrentAnimatorStateInfo(0).length + .5f)
                    _sm.SwitchState(new PlayerAirState());
            }
        }
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
