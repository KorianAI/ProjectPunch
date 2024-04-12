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

            if (other.gameObject.tag == "Enemy") // check in place to avoid errors when hitting scrap piles -J
            other.gameObject.GetComponent<EnemyHealth>().GetStunned(.1f);
        }
    }
}
