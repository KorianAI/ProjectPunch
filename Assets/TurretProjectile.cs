using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public GameObject explosionVFX;
    public LayerMask playerLayer;
    public float damage;
    public float destroyTime = 1f;

    [Header("Hit Ground")]
    public LayerMask whatisGround;
    public GameObject rumbleVFX;

    private void OnCollisionEnter(Collision collision)
    {
        RumbleManager.instance.RumblePulse(.10f, .5f, .10f);
        //sfx - fire

        if (collision.gameObject.tag != "Enemy")
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);

            Collider[] player = Physics.OverlapSphere(transform.position, 2, playerLayer);
            foreach (Collider c in player)
            {
                c.GetComponent<IDamageable>().TakeDamage(damage);
                //sfx - explode
                RumbleManager.instance.RumblePulse(.25f, 1f, .25f);
                
            }

            if (collision.gameObject.layer == whatisGround)
            {
                Instantiate(rumbleVFX, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

        else
        {
            Destroy(gameObject, destroyTime);
        }
    }
}
