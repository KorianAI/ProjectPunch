using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashmereSpotlight : MonoBehaviour
{
    [SerializeField] Cashmere cashmere;

    [SerializeField] int spotlight;
    [SerializeField] GameObject spotlightObj;
    [SerializeField] public Transform bombPoint;

    [SerializeField] GameObject electricVFX;
    [SerializeField] Transform vfxStartPoint;
    [SerializeField] Transform vfxEndPoint;

    [SerializeField] public Transform cmPoint;

    [SerializeField] public LayerMask player;

    public BlastWave smallBlast;

    public float knockback;
    public float kbSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cashmere.currentSpotlight = spotlight;
            cashmere.playerOnSpotlight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cashmere.playerOnSpotlight = false;
        }
    }

    public void Electrocute(float type)
    {
        Debug.Log("bruh");
        // visuals
        spotlightObj.transform.DOShakePosition(1, 1);
        GameObject shockVFX = Instantiate(electricVFX, vfxStartPoint.position, Quaternion.identity);
        if (type == 1) { StartCoroutine(smallBlast.Blast()); }
        
        shockVFX.transform.DOMove(vfxEndPoint.position, 1).OnComplete(() => {  Destroy(shockVFX); });

        StunPlayer();

        // check for player
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, player);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<IDamageable>().TakeDamage(10);
            }
        }
    }

    void StunPlayer()
    {   
        Collider[] colliders = Physics.OverlapSphere(vfxEndPoint.position, 4f, player);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<IDamageable>().TakeDamage(10);
                collider.GetComponent<IKnockback>().Knockback(knockback, transform, kbSpeed);
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.magenta;
    //    Gizmos.DrawWireSphere(vfxEndPoint.position, 4f);
    //}
}
