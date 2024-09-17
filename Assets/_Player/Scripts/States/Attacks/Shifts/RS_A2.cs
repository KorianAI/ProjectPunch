using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RS_A2 : PlayerAirAttack
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
        _sm.pc.SaveAtkIndex(1);
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
            canFall = true;

            if (_sm.ih.GetBufferedInputs().Length > 0)
            {
                _sm.ih.SetCanConsumeInput(true);
            }


            if (fixedtime > duration + .5f)
                _sm.SwitchState(new PlayerAirState());

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
            base.HandleBufferedInput(command);
        }
    }
}