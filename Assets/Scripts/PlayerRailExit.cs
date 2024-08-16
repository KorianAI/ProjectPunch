using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRailExit : PlayerMovementBase
{
    Vector3 movement;
    float dur;
    bool launched;
    public float duration = 0.3f;

    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _sm.anim.Play("Backflip");
        _sm.splineFollower.followDuration = duration;
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        base.FrameUpdate(player);

        movement = (_sm.transform.forward + _sm.transform.up).normalized;

        if (fixedtime > duration) 
        {
            if (!launched)
            {
                _sm.anim.SetBool("onRail", false);
                launched = true;
            }

            _sm.pm.ApplyGravity(3);
        }

        if (player.pm.grounded && player.pm.yVelocity < 0)
        {
            player.SwitchState(player.moveState);
        }
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        base.HandleBufferedInput(command);
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
        if (launched)
        {
            _sm.pm.velocity = _sm.playerObj.forward * _sm.pm.launchForce;
            _sm.pm.velocity.y = _sm.pm.yVelocity;
            _sm.pm.controller.Move(_sm.pm.velocity * Time.deltaTime);
        }
    }
}
