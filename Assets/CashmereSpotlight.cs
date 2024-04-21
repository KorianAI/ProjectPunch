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

    [SerializeField] public LayerMask player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cashmere.currentSpotlight = spotlight;
        }
    }

    public void Electrocute()
    {
        Debug.Log("bruh");
        // visuals
        spotlightObj.transform.DOShakePosition(1, 1);
        GameObject shockVFX = Instantiate(electricVFX, vfxStartPoint.position, Quaternion.identity);
        shockVFX.transform.DOMove(vfxEndPoint.position, 1).OnComplete(() => { Destroy(shockVFX); });

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
}