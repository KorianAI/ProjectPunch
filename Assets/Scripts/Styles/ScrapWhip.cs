using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapWhip : LightStyleInfo
{
    public Animator whipAnim;

    public override void Attack1(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 1");
        player.anim.runtimeAnimatorController = attack1;
        
        whipAnim.Play("WhipAnim");
    }

    public override void Attack2(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 2");
        player.anim.runtimeAnimatorController = attack1;
    }

    public override void Attack3(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 3");
        player.anim.runtimeAnimatorController = attack1;
    }
}
