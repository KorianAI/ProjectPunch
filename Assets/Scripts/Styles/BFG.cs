using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG : WeaponInfo
{
    public override void Start()
    {
        base.Start();
    }

    public override void Input(InputCommand command, bool grounded)
    {
        if (grounded && command.Type == InputType.Y)
        {
            sm.SwitchState(new BFG_G1());
        }

        else
        {
            sm.SwitchState(sm.inAirState);
        }
    }
}
