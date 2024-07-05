using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG : Mode
{
    public override void Start()
    {
        base.Start();
    }

    public override void WeaponInput(InputCommand command, bool grounded, int index)
    {
        if (grounded)
        {
            if (command.Type == InputType.Y)
            {
                BaseCombo(index);   
            }
        }

        else
        {
            if (command.Type == InputType.Y)
            {
                AirCombo(index);
            }
        }
    }

    public void BaseCombo(int index)
    {
        stats = baseComboStats[index];

        if (index == 0)
        {           
            sm.SwitchState(new BFG_G1());
        }

        else if (index == 1)
        {
            if (sm.pc.pauseAttack)
            {
                sm.SwitchState(new BFG_G4());
            }

            else
            {
                sm.SwitchState(new BFG_G2());
            }

        }

        else if (index == 2)
        {
            sm.SwitchState(new BFG_G3());
        }

        else if (index == 3)
        {
            sm.SwitchState(new BFG_G5());
        }
    }

    public void AirCombo(int index)
    {
        stats = airComboStats[index];

        if (index == 0)
        {
            sm.SwitchState(new BFG_A1());
        }

        else if (index == 1)
        {
            sm.SwitchState(new BFG_A2());
        }

        else if (index == 2)
        {
            sm.SwitchState(new BFG_A3());
        }
    }
}