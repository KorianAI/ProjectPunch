using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullState : PlayerAttackBase
{
    public override void EnterState(PlayerStateManager player)
    {

        duration = .4f;
        rangeAttack = true;
        base.EnterState(player);
        _sm.anim.Play("Pull");
        _sm.StartCoroutine(TargetPull());
        _sm.pulling = true;
        _sm.pc.ResetAirGrav();
        canAttack = false;
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        if (fixedtime > duration)
        {
            canAttack = true;

            if (_sm.ih.GetBufferedInputs().Length > 0)
            {
                _sm.ih.SetCanConsumeInput(true);
            }

            else
            {
                if (fixedtime > animator.GetCurrentAnimatorStateInfo(0).length + .1f)
                    _sm.SwitchState(new PlayerIdleState());
            }
        }
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        if (!_sm.pulling || canAttack)
        {
            base.HandleBufferedInput(command);
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

    public IEnumerator TargetPull()
    {
        var target = _sm.tl.currentTarget.gameObject;
        yield return new WaitForSeconds(.2f);
        target.GetComponent<IMagnetisable>().Pull(_sm);
    }
}
