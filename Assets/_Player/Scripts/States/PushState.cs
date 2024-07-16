using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : PlayerAttackBase
{
    GameObject target;
    public override void EnterState(PlayerStateManager player)
    {

        duration = .2f;
        base.EnterState(player);
        _sm.anim.Play("Push");
        _sm.StartCoroutine(TargetPush());
        _sm.pushing = true;
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
        if (!_sm.pushing || canAttack)
        {
            base.HandleBufferedInput(command);
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

    public IEnumerator TargetPush()
    {
        yield return new WaitForSeconds(.2f);
        if (_sm.tl.currentTarget != null)
        {
            target = _sm.tl.currentTarget.gameObject;
        }
        target.GetComponent<IMagnetisable>().Push(_sm);
    }
}
