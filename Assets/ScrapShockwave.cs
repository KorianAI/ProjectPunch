using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapShockwave : MonoBehaviour
{
    public bool canDealDamage = true;
    public float knockupPower;
    public float length;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject + "1");
        var target = collision.gameObject.GetComponentInParent<IDamageable>();

        if (collision.CompareTag("Player") && target != null)
        {
            if (canDealDamage)
            {
                target.TakeDamage(10);
                canDealDamage = false;
                collision.gameObject.GetComponentInParent<IKnockback>().Knockback(knockupPower, transform, length);
            }

        }
    }
}
