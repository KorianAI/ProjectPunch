using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public GameObject explosionVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
