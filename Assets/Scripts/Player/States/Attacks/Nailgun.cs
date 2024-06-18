using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nailgun : WeaponInfo
{
    public override void Start()
    {
        base.Start();
    }

    public override void Input(InputCommand command, bool grounded)
    {
        if (grounded && command.Type == InputType.X)
        {
            sm.SwitchState(new NLG_G1());
        }

        else
        {
            sm.SwitchState(sm.inAirState);
        }
    }


}
