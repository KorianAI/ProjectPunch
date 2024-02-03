using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttack : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.canAttack = false;
        player.animHandler.ChangeAnimationState("PlayerHeavyAttack");
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        
    }
}
