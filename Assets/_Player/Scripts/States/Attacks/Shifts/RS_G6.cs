using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RS_G6 : PlayerGroundAttack
{
    public override void EnterState(PlayerStateManager player)
    {
        atkMoveDistance = 2;
        atkMoveDur = .7f;
        duration = .7f;
        base.EnterState(player);
        player.anim.Play("RS6");
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

            if (fixedtime > animator.GetCurrentAnimatorStateInfo(0).length - .5f)
                _sm.SwitchState(new PlayerIdleState());

        }
    }

    public override void HandleBufferedInput(InputCommand command)
    {

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }
}
