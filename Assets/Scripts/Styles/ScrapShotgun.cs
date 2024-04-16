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

    [Header("UI")]
    [SerializeField] SlotManager ammoUI;

    public bool canAddAmmo = true;

    private void Start()
    {
        shells = maxShells;
        ammoUI.currentValue = shells * 10;
        ammoUI.maxValue = maxShells * 10;
    }

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
            player.CheckForEnemies();
            ChangeAmmo(-1f);
        }
    }

    public void ChangeAmmo(float amount)
    {
        if (amount > 0 && canAddAmmo) { shells += amount; StartCoroutine(CanAddAmmo()); }
        else if (amount < 0) { shells += amount; }
        if (shells > maxShells)
        {
            shells = maxShells;
        }
        ammoUI.currentValue = shells * 10;
        ammoUI.DrawSlots();
    }

    IEnumerator CanAddAmmo()
    {
        canAddAmmo = false;
        yield return new WaitForSeconds(1f);
        canAddAmmo = true;
    }
}
