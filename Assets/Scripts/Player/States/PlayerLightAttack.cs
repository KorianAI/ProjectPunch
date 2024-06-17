using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightAttack : PlayerAttackBase
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
            if (_sm.inputHandler.GetBufferedInputs().Length > 0)
            {
                _sm.inputHandler.SetCanConsumeInput(true);
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
        if (time > duration)
        {
            if (command.Type == InputType.Y)
            {
                Debug.Log("Heavy Attack received in light state");
                _sm.SwitchState(new PlayerHeavyAttack());
            }

            else if (command.Type == InputType.X)
            {
                _sm.SwitchState(new PlayerLightAttack());
                Debug.Log("Light Attack received in light state");
            }
        }
    }
}
