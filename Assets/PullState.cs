using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullState : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {

        base.EnterState(player);
        if (!_sm.pm.grounded) { _sm.anim.SetBool("AirAttack", true); }
        _sm.anim.Play("Pull");
        _sm.StartCoroutine(TargetPull());
        _sm.pulling = true;
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {

    }

    public override void HandleBufferedInput(InputCommand command)
    {

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

    public IEnumerator TargetPull()
    {
        yield return new WaitForSeconds(.2f);

        if (_sm.tl.currentTarget != null)
        {
            var target = _sm.tl.currentTarget.gameObject;
            target.GetComponent<IMagnetisable>().Pull(_sm);
        }
    }
}
