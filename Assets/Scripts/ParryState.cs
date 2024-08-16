using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryState : PlayerState
{
    GameObject target;

    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _sm.anim.Play("Parry");
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {

        if (_sm.ih.GetBufferedInputs().Length > 0)
        {
            _sm.ih.SetCanConsumeInput(true);
        }

        else
        {
            if (fixedtime > _sm.anim.GetCurrentAnimatorStateInfo(0).length)
                _sm.SwitchState(new PlayerIdleState());
        }

    }

    public override void HandleBufferedInput(InputCommand command)
    {
        if (!_sm.pushing)
        {
            base.HandleBufferedInput(command);
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }


}
