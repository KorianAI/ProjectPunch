using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RS_G4 : PlayerGroundAttack
{
    public override void EnterState(PlayerStateManager player)
    {
        rangeAttack = false;
        atkMoveDistance = 2.25f;
        atkMoveDur = .4f;
        duration = .3f;
        base.EnterState(player);
        player.anim.Play("RS4");
        canAttack = false;
        _sm.pc.SaveAtkIndex(3);

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
            base.HandleBufferedInput(command);
        }
    }
}
