using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCollider : MonoBehaviour
{

    PlayerCombat pc;

    private void Start()
    {
        pc = GetComponentInParent<PlayerCombat>();
    }
    private void OnTriggerEnter(Collider other)
    {
        IParriable parryObj = other.GetComponent<IParriable>();

        if (parryObj != null)
        {
            parryObj.Parry();
            pc.ParryEffect(other.gameObject);
            pc._sm.parryParticles.Play();
        }
    }
}
