using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendoArms : ShiftInfo
{
    // HEAVY COMBO

    public override void HAttack1(float damage, float range)
    {
        player.anim.runtimeAnimatorController = hAttack1;
    }

    public override void HAttack2(float damage, float range)
    {
        player.anim.runtimeAnimatorController = hAttack2;
    }

    public override void HAttack3(float damage, float range)
    {
        player.anim.runtimeAnimatorController = hAttack3;
    }

    // LIGHT COMBO

    public override void LAttack1(float damage, float range)
    {
        player.anim.runtimeAnimatorController = lAttack1;
    }

    public override void LAttack2(float damage, float range)
    {
        player.anim.runtimeAnimatorController = lAttack2;
    }

    public override void LAttack3(float damage, float range)
    {
        player.anim.runtimeAnimatorController = lAttack3;
    }
}
