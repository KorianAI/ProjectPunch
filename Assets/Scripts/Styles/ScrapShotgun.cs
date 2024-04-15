using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapShotgun : HeavyStyleInfo
{
    [Header("ShotgunShells")]
    public float shells;
    public float maxShells;

    public GameObject shotgunVFX;
    public Transform vfxSpawn;


    public override void Attack1(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 1");
        player.anim.runtimeAnimatorController = attack1;
    }

    public override void Attack2(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 2");
        player.anim.runtimeAnimatorController = attack2;
    }

    public override void Attack3(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 3");
        player.anim.runtimeAnimatorController = attack3;
    }

    public void ShotgunBlast()
    {
        Debug.Log("boom :3");
        if (shells > 0)
        {
            Instantiate(shotgunVFX, vfxSpawn.position, Quaternion.identity);
            shells--;
        }
    }
}
