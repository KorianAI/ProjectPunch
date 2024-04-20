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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cashmere.currentSpotlight = spotlight;
        }
    }

    public void Electrocute()
    {
        spotlightObj.transform.DOShakePosition(1, 1);
    }
}
