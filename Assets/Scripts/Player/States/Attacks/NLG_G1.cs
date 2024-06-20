using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NLG_G1 : PlayerAttackBase
{

    public override void EnterState(PlayerStateManager player)
    {   
        
        base.EnterState(player);
        duration = 0.5f;
        player.anim.SetTrigger("LightAttack");
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
            if (command.Type == InputType.Y)
            {
                Debug.Log("Heavy Attack received in light state");
                _sm.SwitchState(new BFG_G1());
            }

            else if (command.Type == InputType.X)
            {
                _sm.SwitchState(new NLG_G1());
                Debug.Log("Light Attack received in light state");
            }

            else
            {
                base.HandleBufferedInput(command);
            }
        }

    }
}
