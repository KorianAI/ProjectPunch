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
                // RS_X // change to shockwave
            }

            else if (command.Type == InputType.xH)
            {
                // RS_XH; // change to better nail
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
            // RS_1 - Punch 1
        }

        else if (index == 1)
        {
            if (sm.pc.pauseAttack)
            {
                // RS_4 - Barrage 1
            }

            else
            {
                // RS_2 - Punch 2
            }

        }

        else if (index == 2)
        {
            if (sm.pc.pauseAttack)
            {
                // RS_6 - Haymaker
            }

            else
            {
                // RS_3 - Punch 3
            }

        }

        else if (index == 3)
        {
            // RS_5 - Barrage 2
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
