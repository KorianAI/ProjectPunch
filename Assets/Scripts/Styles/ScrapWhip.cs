using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapWhip : LightStyleInfo
{
    public override void Attack1(float damage, float range)
    {
        Debug.Log(name + ": Attack 1");
    }

    public override void Attack2(float damage, float range)
    {
        Debug.Log(name + ": Attack 2");
    }

    public override void Attack3(float damage, float range)
    {
        Debug.Log(name + ": Attack 3");
    }
}
