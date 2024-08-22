using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class PlayerAirAttack : PlayerAttackBase
{
    public float moveUpDistance = .6f; // Adjust the upward movement distance as needed
    public float moveUpDuration = 0.1f; // Adjust the duration of the upward movement
    public bool canFall = false;

    float maxPoint;

    public override void EnterState(PlayerStateManager player)
    {

        airAttack = true;
        base.EnterState(player);
        _sm.anim.SetBool("AirAttack", true);
        _sm.pm.moveDirection = Vector3.zero;
        float targetYPosition = player.transform.position.y + moveUpDistance;
        if (targetYPosition > _sm.pc.yPosition) { targetYPosition = _sm.pc.yPosition; };
        player.transform.DOMoveY(targetYPosition, moveUpDuration).SetEase(Ease.OutQuad);
        _sm.pc.AirAttackIncrement(1);

    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        base.FrameUpdate(player);
        if (_sm.pc.airAtkGravity)
        {
            _sm.pm.ApplyGravity(2);
        }

    }

    public override void HandleBufferedInput(InputCommand command)
    {
        base.HandleBufferedInput(command);
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
        if (_sm.pc.airAtkGravity)
        {
            player.pm.velocity.y = player.pm.yVelocity;
            player.pm.controller.Move(player.pm.velocity * Time.deltaTime);
        }
    }
}
