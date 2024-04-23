using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapWhip : LightStyleInfo
{
    PlayerAudioManager audioManager;


    private void OnEnable()
    {
        PlayerResources.enterScrapStyle += ShowWeapon;
        PlayerResources.exitScrapStyle += HideWeapon;

        audioManager = GetComponentInParent<PlayerAudioManager>();
    }

    private void OnDisable()
    {
        PlayerResources.enterScrapStyle -= ShowWeapon;
        PlayerResources.exitScrapStyle -= HideWeapon;
    }
    public override void Attack1(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 1");
        player.anim.runtimeAnimatorController = attack1;
        audioManager.BaseAttackMetallic();
    }

    public override void Attack2(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 2");
        player.anim.runtimeAnimatorController = attack2;
        audioManager.BaseAttackMetallic();
    }

    public override void Attack3(float damage, float range)
    {
        Debug.Log(styleName + ": Attack 3");
        player.anim.runtimeAnimatorController = attack3;
        audioManager.BaseAttackMetallic();
    }

    void ShowWeapon()
    {

        weaponObject.SetActive(true);
    }

    void HideWeapon()
    {

        weaponObject.SetActive(false);
    }

}
