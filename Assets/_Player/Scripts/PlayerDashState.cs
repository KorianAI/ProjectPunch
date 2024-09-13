using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerMovementBase
{
    private float dashDistance = 5f; 
    private float dashDuration = 0.3f; 
    private Vector3 dashDirection; 

    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);

        player.transform.DOKill();

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = _sm.orientation.forward * verticalInput + _sm.orientation.right * horizontalInput;
        if (inputDir == Vector3.zero)
        {
            inputDir = _sm.playerObj.transform.forward; // Default to forward if no input
        }

        _sm.playerObj.transform.forward = inputDir.normalized;

        if (_sm.resources.scrapShift)
        {

            player.transform.DOMove(player.transform.position + player.playerObj.transform.forward * dashDistance, .15f)
                     .SetEase(Ease.OutQuad)
                     .OnComplete(() => OnDashComplete());

            _sm.resources.shift.UpdateOverdrive(1);

        }

        else
        {

            player.transform.DOMove(player.transform.position + player.playerObj.transform.forward * dashDistance, dashDuration)
                     .SetEase(Ease.OutQuad)
                     .OnComplete(() => OnDashComplete());
        }

        _sm.anim.Play("PlayerDash");
        _sm.ih.SetCanConsumeInput(true);

        if (_sm.resources.shift.overdrive)
        {
            _sm.pm.DashEffect(_sm.pm.rsxDashPrefab, new Vector3(0, 90, 0));
        }

        else
        {
            _sm.pm.DashEffect(_sm.pm.dashEffectPrefab, new Vector3(0, 90, 0));
        }


        if (!_sm.pm.grounded)
        {
            _sm.pc.ResetAirGrav();
        }
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

        if (fixedtime > dashDuration)
        {
            base.HandleBufferedInput(command);
        }

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

    void OnDashComplete()
    {
        if (_sm.pm.grounded)
        {
            _sm.SwitchState(new PlayerIdleState());
        }

        else
        {
            _sm.SwitchState(new PlayerAirState());
        }

    }
}
