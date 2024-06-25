using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerMovementBase
{
    private float dashDistance = 5f; 
    private float dashDuration = 0.3f; 
    private Vector3 dashDirection; 

    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        _sm.cam.canRotate = false;
        player.transform.DOMove(player.transform.position + player.playerObj.transform.forward * dashDistance, dashDuration)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() => OnDashComplete());
        _sm.anim.SetTrigger("Dash");
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

    void OnDashComplete()
    {
        _sm.SwitchState(new PlayerIdleState());
    }
}
