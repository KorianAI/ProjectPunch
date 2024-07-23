using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounceState : PlayerMovementBase
{
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);

        _sm.tl.ResetTarget();
        _sm.cam.canRotate = true;
        PlayerCameraManager.instance.SwitchPlayerCam();
        _sm.anim.Play("Backflip");

        //speed lines, pull out cam?
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        base.FrameUpdate(player);
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
