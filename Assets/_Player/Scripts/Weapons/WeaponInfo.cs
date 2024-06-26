using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    public PlayerStateManager sm;
    public AttackStats stats;


    public virtual void Start()
    {
        sm = GetComponentInParent<PlayerStateManager>();
    }

    public virtual void WeaponInput(InputCommand command, bool grounded, int index)
    {

    }
}
