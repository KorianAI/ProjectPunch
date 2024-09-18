using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailTriggerArea : MonoBehaviour
{
    private GameObject player;
    private TargetCams tc;
    private float initMaxDist;
    public float newMaxDist = 100f;

    void Start()
    {
        player = GameObject.Find("Player");
        tc = player.GetComponent<TargetCams>();
        initMaxDist = tc.maxDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tc.maxDistance = newMaxDist;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tc.maxDistance = initMaxDist;
        }
    }
}
