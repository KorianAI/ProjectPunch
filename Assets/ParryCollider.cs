using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IParriable parryObj = other.GetComponent<IParriable>();

        if (parryObj != null)
        {
            parryObj.Parry();
        }
    }
}
