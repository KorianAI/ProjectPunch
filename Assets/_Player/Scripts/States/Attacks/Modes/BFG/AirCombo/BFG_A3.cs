using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG_A3 : PlayerAirAttack
{
    public override void EnterState(PlayerStateManager player)
    {
        rangeAttack = false;
        atkMoveDistance = 2.25f;
        atkMoveDur = .4f;
        duration = .7f;
        base.EnterState(player);
        player.anim.Play("AirSlam");
        canAttack = false;
        _sm.pc.SaveAtkIndex(0);
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
           
            canFall = true;
            if (fixedtime > animator.GetCurrentAnimatorStateInfo(0).length)
                _sm.SwitchState(new PlayerAirState());

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
