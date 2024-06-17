using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class WeaponInfo : MonoBehaviour
{
    public PlayerStateManager sm;

    public virtual void Start()
    {
        sm = GetComponentInParent<PlayerStateManager>();
    }

    public abstract void Input(InputCommand command, bool grounded);   
}
