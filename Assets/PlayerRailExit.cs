using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRailExit : PlayerMovementBase
{
    Vector3 movement;

    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _sm.anim.Play("Backflip");
        _sm.pm.launchDirection = (_sm.playerObj.transform.forward + _sm.playerObj.transform.up).normalized;
        //HitstopManager.Instance.AlterTimeScale(0.5f, .1f);
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        base.FrameUpdate(player);
        _sm.pm.ApplyGravity(3);
        movement = _sm.pm.launchDirection * _sm.pm.launchForce * Time.deltaTime;

        if (fixedtime > _sm.pm.launchDuration)
        {
            _sm.SwitchState(new PlayerAirState());
            _sm.anim.SetBool("onRail", false);
            //HitstopManager.Instance.AlterTimeScale(1, .25f);
        }
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        base.HandleBufferedInput(command);
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
        _sm.controller.Move(movement);
    }
}
