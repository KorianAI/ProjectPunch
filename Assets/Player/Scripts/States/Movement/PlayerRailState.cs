using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerRailState : PlayerState
{
    public PlayerStateManager ps;

    public override void EnterState(PlayerStateManager player)
    {
        ps = player;
        ps.cam.canRotate = false;
        
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.anim.SetBool("onRail", false);
        ps.cam.canRotate = true;
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        
    }


    public override void PhysicsUpdate(PlayerStateManager player)
    {
        
    }

    public override void HandleBufferedInput(InputCommand command)
    {

    }
}
