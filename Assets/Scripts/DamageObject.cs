using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damage;
    public GameObject hitVFX;

    
    public bool extendoFists;
    public GameObject projectile;

    public bool whip;
    public LayerMask enemyLayer;


    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();

        if (target != null && !other.gameObject.CompareTag("Player"))
        {
            target.TakeDamage(damage);
            Instantiate(hitVFX, transform.position, Quaternion.identity);

            if (other.gameObject.tag == "Enemy") // check in place to avoid errors when hitting scrap piles -J
                other.gameObject.GetComponent<EnemyHealth>().GetStunned(.1f);

            if (whip)
            {
                IncreaseShotgunAmmo();
            }

            if (extendoFists)
            {
                LaunchProjectile(other.transform, other.gameObject);
            }
          
        }

        else if (other.CompareTag("Player"))
        {
            target.TakeDamage(15f);
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

            // Calculate direction towards the random enemy
            Vector3 targetPosition = randomEnemy.transform.position;

            // Use DOTween to move the projectile towards the target
            proj.transform.DOMove(targetPosition, .3f)
                .SetEase(Ease.Linear)
                .OnComplete(() => h.Detonate()); // Destroy the projectile when the tween is complete

        }
    }

    private void IncreaseShotgunAmmo()
    {
        var parent = transform.root;
        ScrapShotgun shotgun = parent.GetComponentInChildren<ScrapShotgun>();
        if (shotgun != null)
        {
            Debug.Log("uwu");
            shotgun.ChangeAmmo(1);
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
