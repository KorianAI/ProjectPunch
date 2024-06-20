using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG_G3 : PlayerAttackBase
{
    public override void EnterState(PlayerStateManager player)
    {
        atkMoveDistance = 2;
        atkMoveDur = .7f;
        base.EnterState(player);
        duration = .7f;
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
        if (fixedtime > animator.GetCurrentAnimatorStateInfo(0).length)
        {
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
