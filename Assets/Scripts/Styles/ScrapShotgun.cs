using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapShotgun : HeavyStyleInfo
{
    public override void Attack1(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 1");
    }

    public override void Attack2(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 2");
    }

    public override void Attack3(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 3");
    }
}
