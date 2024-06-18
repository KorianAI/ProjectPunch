using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAttackBase : PlayerState
{
    public float duration;
    protected Animator animator;
    protected bool canAttack = false;
    public int attackIndex;

    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        animator = GetComponent<Animator>();
        _sm.inputHandler.SetCanConsumeInput(false);
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
        if (command.Type == InputType.A)
        {
            _sm.pm.Jump();
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }


}
