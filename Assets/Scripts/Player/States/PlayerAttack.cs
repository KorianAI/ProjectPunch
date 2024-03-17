using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.canAttack = false;
        player.anim.Play("PlayerLightAttack");
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.anim.Play("PlayerIdle");
    }

    public override void FrameUpdate(PlayerStateManager player)
    {

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {

    }
}
