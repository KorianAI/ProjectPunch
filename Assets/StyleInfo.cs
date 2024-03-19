using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class StyleInfo : MonoBehaviour
{
    public GameObject weaponObject;
    public Sprite icon;
    public string styleName;
    public float currentDurability;

    public float atkSpeed;
    public float atkDmg;
    public float range;

    public AnimatorOverrideController attack1;
    public AnimatorOverrideController attack2;
    public AnimatorOverrideController attack3;


    public abstract void Attack1(float damage, float range);
    public abstract void Attack2(float damage, float range);
    public abstract void Attack3(float damage, float range);
}
