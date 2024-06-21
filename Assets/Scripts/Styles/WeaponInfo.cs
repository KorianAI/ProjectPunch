using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class WeaponInfo : MonoBehaviour
{
    public PlayerStateManager sm;
    public AttackStats[] stats;

    public virtual void Start()
    {
        sm = GetComponentInParent<PlayerStateManager>();
    }

    public abstract void WeaponInput(InputCommand command, bool grounded);   
}
