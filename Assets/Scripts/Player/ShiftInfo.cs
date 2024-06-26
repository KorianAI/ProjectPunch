using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShiftInfo : MonoBehaviour
{
    public PlayerStateManager player;
    public GameObject weaponObject;
    public Sprite icon;
    public string styleName;
    public float currentDurability;

    public float atkSpeed;
    public float atkDmg;
    public float range;

    public AnimatorOverrideController lAttack1;
    public AnimatorOverrideController lAttack2;
    public AnimatorOverrideController lAttack3;

    public AnimatorOverrideController hAttack1;
    public AnimatorOverrideController hAttack2;
    public AnimatorOverrideController hAttack3;


    public abstract void LAttack1(float damage, float range);
    public abstract void LAttack2(float damage, float range);
    public abstract void LAttack3(float damage, float range);

    public abstract void HAttack1(float damage, float range);
    public abstract void HAttack2(float damage, float range);
    public abstract void HAttack3(float damage, float range);

}
