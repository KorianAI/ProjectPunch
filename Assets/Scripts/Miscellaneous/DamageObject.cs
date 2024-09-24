using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damage;
    public GameObject hitVFX;
    public GameObject projectile;
    public LayerMask enemyLayer;


    private void OnTriggerEnter(Collider other)
    {
       


        if (other.gameObject.CompareTag("Enemy"))
        {
            var target = other.gameObject.GetComponent<IDamageable>();
            if (target!= null)
            {
                target.TakeDamage(damage, true);
            }
            Instantiate(hitVFX, transform.position, Quaternion.identity);

            GetComponent<SphereCollider>().enabled = false;
        }
    }

    void LaunchProjectile(Transform hitPos, GameObject hitEnemy)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPos.position, 5, enemyLayer);
        colliders = RemoveHitEnemy(colliders, hitEnemy);

        Debug.Log(hitEnemy + "1");

        if (colliders.Length > 0)
        {
            // Choose a random enemy from the list
            GameObject randomEnemy = colliders[Random.Range(0, colliders.Length)].gameObject;
            Debug.Log(randomEnemy + "2");

            // Instantiate projectile at the hit point
            GameObject proj = Instantiate(projectile, hitPos.position, Quaternion.identity);
            HomingProjectile h = proj.GetComponent<HomingProjectile>();

            // Calculate enemy towards the random enemy
            Vector3 targetPosition = randomEnemy.transform.position;

            // Use DOTween to move the projectile towards the target
            proj.transform.DOMove(targetPosition, .3f)
                .SetEase(Ease.Linear)
                .OnComplete(() => h.Detonate()); // Destroy the projectile when the tween is complete

        }
    }

    Collider[] RemoveHitEnemy(Collider[] colliders, GameObject hitEnemy)
    {
        // Create a list to store the filtered colliders
        var filteredColliders = new List<Collider>();

        // Iterate through the colliders and add them to the list if they're not the hit enemy
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != hitEnemy)
            {
                filteredColliders.Add(collider);
            }
        }

        // Convert the list back to an array
        return filteredColliders.ToArray();
    }
}
