using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightAttack : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.canAttack = false;
        player.animHandler.ChangeAnimationState("PlayerLightAttack");
    }

    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Bruh");
    }

    public override void FrameUpdate(PlayerStateManager player)
    {

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {

    }
}
