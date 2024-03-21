using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();

        if (target != null && !other.gameObject.CompareTag("Player"))
        {
            target.TakeDamage(damage);
        }
    }
}
