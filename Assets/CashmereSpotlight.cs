using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashmereSpotlight : MonoBehaviour
{
    [SerializeField] Cashmere cashmere;
    [SerializeField] int spotlight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cashmere.currentSpotlight = spotlight;
        }
    }
}
