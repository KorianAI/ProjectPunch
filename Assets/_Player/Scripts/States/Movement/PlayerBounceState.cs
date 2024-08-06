using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounceState : PlayerMovementBase
{
    Bouncepad currentPad;

    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _sm.bouncing = true;
        _sm.anim.SetBool("Bouncing", true);
        _sm.anim.Play("Backflip");
        _sm.pm.moveDirection = Vector3.zero;
        _sm.pm.velocity = Vector3.zero;

        currentPad = player.tl.targetable.GetComponent<Bouncepad>();
        _sm.splineFollower.followDuration = currentPad.duration;
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);

        _sm.cam.canRotate = true;
        PlayerCameraManager.instance.SwitchPlayerCam();
        _sm.pushing = false;
        _sm.bouncing = false;
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        base.FrameUpdate(player);
        if (fixedtime > currentPad.duration)
        {
            _sm.SwitchState(new PlayerAirState());
            _sm.anim.SetBool("Bouncing", false);
        }
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        base.HandleBufferedInput(command);
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }
}
