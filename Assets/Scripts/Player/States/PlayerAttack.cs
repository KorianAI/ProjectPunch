using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAttack : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        if (player.comboCounter < 2)
        {
            player.transform.DOMove(player.transform.position + (player.playerObj.forward * player.attackMoveDistance), .5f);
        }       
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        if (!player.grounded)
        {
            player.transform.DOMove(player.transform.position + (player.playerObj.forward), .5f);
            player.SwitchState(player.inAirState);
            player.canAttack = true;
            player.cam.canRotate = true;
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {

    }
}
