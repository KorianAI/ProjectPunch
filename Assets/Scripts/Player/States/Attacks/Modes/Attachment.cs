using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : WeaponInfo
{
    public override void Start()
    {
        base.Start();
    }

    public override void WeaponInput(InputCommand command, bool grounded, int index)
    {
        base.WeaponInput(command, grounded, index);
    }
}
