using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRailState : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.anim.Play("Hang");
        player.anim.SetBool("onRail", true);
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.anim.SetBool("onRail", false);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        
    }
}
