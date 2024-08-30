using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapVolleyProjectile : MonoBehaviour, IMagnetisable
{
    public float damage;
    public LayerMask collision;
    public GameObject rumbleVFX;
    public GameObject scrapPile;

    public Cashmere cm;
    public bool beenParried;

    bool playerHit;

    public AudioSource source;

    public void SpawnScrapPile()
    {   
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, collision);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<IDamageable>().TakeDamage(damage);
                playerHit = true;
            }
        }

        if (colliders.Length > 0 && !playerHit)
        {
            foreach (Collider collider in colliders)
            {
                
                Instantiate(scrapPile, transform.position, transform.rotation);
                Debug.Log(collider.gameObject);
                break;
            }
        }

        GameObject rumbleEffect = Instantiate(rumbleVFX, transform.position, transform.rotation);

        playerHit = false;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    public void Pull(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }

    public void Push(PlayerStateManager player)
    {

        transform.DOKill();
        transform.DOMove(cm.transform.position, .4f).OnComplete(() => { cm.health.TakeDamage(68); GameObject rumbleEffect = Instantiate(rumbleVFX, transform.position, transform.rotation); gameObject.SetActive(false); });
    }
}
