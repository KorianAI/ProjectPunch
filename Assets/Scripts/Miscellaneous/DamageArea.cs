using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public float playerDamage;
    public float enemyDamage;
    public float knockbackDist;
    public float knockbackLength;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit" + other.name);
        
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("calling player");
            
            other.gameObject.GetComponent<PlayerResources>().TakeDamage(playerDamage, false);

            other.gameObject.GetComponent<PlayerStateManager>().Knockback(knockbackDist, transform, knockbackLength);
        }

        else if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("calling enemy");
            
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(enemyDamage, false);
        }
    }
}
