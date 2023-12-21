using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Attack State");
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
