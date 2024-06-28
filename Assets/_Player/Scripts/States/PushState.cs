using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : PlayerAttackBase
{
    bool canFall = false;

    public override void EnterState(PlayerStateManager player)
    {
        duration = .2f;
        base.EnterState(player);
        if (!_sm.pm.grounded) { _sm.anim.SetBool("AirAttack", true); }
        _sm.anim.Play("Push");
        _sm.pc.ResetAirGrav();
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        if (fixedtime > duration)
        {
            canAttack = true;
            canFall = true; 

            if (_sm.ih.GetBufferedInputs().Length > 0)
            {
                _sm.ih.SetCanConsumeInput(true);
            }

            else
            {
                if (fixedtime > duration + .5f)
                {
                    _sm.pm.ApplyGravity(3);

                    if (_sm.pm.grounded)
                    {
                        _sm.SwitchState(new PlayerIdleState());
                    }

                    else
                    {
                        _sm.SwitchState(new PlayerAirState());
                    }
                }
            }
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
        if (canFall)
        {
            player.pm.velocity.y = player.pm.yVelocity;
            player.pm.controller.Move(player.pm.velocity * Time.deltaTime);
        }
    }
}
