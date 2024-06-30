using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG_G5 : PlayerGroundAttack
{
    public override void EnterState(PlayerStateManager player)
    {
        atkMoveDistance = 2;
        atkMoveDur = .7f;
        duration = 1f;
        base.EnterState(player);
        player.anim.SetTrigger("HeavyAttack5");
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

            if (fixedtime > animator.GetCurrentAnimatorStateInfo(0).length)
                _sm.SwitchState(new PlayerIdleState());

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
