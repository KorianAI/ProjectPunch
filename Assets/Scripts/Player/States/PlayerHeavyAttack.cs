using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttack : PlayerAttackBase
{
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        duration = 0.5f;
        player.anim.SetTrigger("HeavyAttack");
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        if (fixedtime >= .8f)
        {
            _sm.SwitchState(new PlayerIdleState());
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        if (fixedtime > duration)
        {
            if (command.Type == InputType.X)
            {
                _sm.SwitchState(new PlayerLightAttack());
                Debug.Log("Light Attack received in heavy state");
            }

            else if (command.Type == InputType.Y)
            {
                Debug.Log("Heavy Attack received in heavy state");
                _sm.SwitchState(new PlayerHeavyAttack());
            }
        }
    }
}
