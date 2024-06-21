using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG_G1 : PlayerAttackBase
{
    public override void EnterState(PlayerStateManager player)
    {
        atkMoveDistance = 2.25f;
        atkMoveDur = .4f;
        duration = .4f;
        base.EnterState(player);
        player.anim.SetTrigger("HeavyAttack1");
        canAttack = false;

    }

    public override void ExitState(PlayerStateManager player)
    {
        
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
                if (fixedtime > animator.GetCurrentAnimatorStateInfo(0).length)
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
            if (command.Type == InputType.X)
            {
                _sm.resources.attachment.WeaponInput(command, _sm.pm.grounded);
            }

            else if (command.Type == InputType.Y)
            {
                Debug.Log("Heavy Attack received in heavy state");
                _sm.SwitchState(new BFG_G2());
            }

            else
            {
                base.HandleBufferedInput(command);
            }
        }
    }
}
