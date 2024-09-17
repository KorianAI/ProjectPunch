using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RS_XH : PlayerAttackBase
{

    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        duration = 0.5f;
        _sm.anim.Play("RSXH");
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
            if (_sm.ih.GetBufferedInputs().Length > 0)
            {
                _sm.ih.SetCanConsumeInput(true);
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
            base.HandleBufferedInput(command);
        }

    }
}
