using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttack : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.canAttack = false;
        player.anim.Play("PlayerHeavyAttack");
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

    public override void HandleBufferedInput(InputCommand command)
    {

    }
}
