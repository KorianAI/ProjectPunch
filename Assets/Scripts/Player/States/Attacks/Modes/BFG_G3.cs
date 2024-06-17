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
