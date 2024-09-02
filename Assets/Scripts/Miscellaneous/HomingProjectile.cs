using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public LayerMask enemyLayer;
    public GameObject explosionVFX;

    public void Detonate()
    {
        GameObject hitParticle = Instantiate(explosionVFX, transform.position, Quaternion.identity);

        Collider[] enemies = Physics.OverlapSphere(transform.position, 2, enemyLayer);
        foreach (Collider c in enemies)
        {
            c.GetComponent<IDamageable>().TakeDamage(10);         
            //RumbleManager.instance.RumblePulse(.25f, 1f, .25f);
        }
        Destroy(gameObject);
    }
}
