using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealSteel : WeaponInfo
{
    // Overdrive
    bool overdrive;
    float maxOverdrive;
    float currentOverdrive;

    public override void WeaponInput(InputCommand command, bool grounded, int index)
    {

        if (grounded)
        {
            if (command.Type == InputType.Y)
            {
                BaseCombo(index);
            }

            else if (command.Type == InputType.X)
            {
                // RS_X // change to shockwave - Nailgun
            }

            else if (command.Type == InputType.xH)
            {
                // RS_XH; // change to better nail - Nailgun
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
            sm.SwitchState(new RS_G1());
        }

        else if (index == 1)
        {
            if (sm.pc.pauseAttack)
            {
                sm.SwitchState(new RS_G4());
            }

            else
            {
                sm.SwitchState(new RS_G2());
            }

        }

        else if (index == 2)
        {
            if (sm.pc.pauseAttack)
            {
                sm.SwitchState(new RS_G6());
            }

            else
            {
                sm.SwitchState(new RS_G3());
            }

        }

        else if (index == 3)
        {
            sm.SwitchState(new RS_G5());
        }
    }

    public void AirCombo(int index)
    {
        stats = airComboStats[index];

        if (index == 0)
        {
            // RS_A1
        }

        else if (index == 1)
        {
            // RS_A2
        }

        else if (index == 2)
        {
            // RS_A3
        }
    }
}
