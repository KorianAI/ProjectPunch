using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeavyStyleInfo : StyleInfo
{
    public abstract override void Attack1(float damage, float range);
    public abstract override void Attack2(float damage, float range);
    public abstract override void Attack3(float damage, float range);
}
