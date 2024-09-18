using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    CheckpointManager manager;
    public GameObject point;
    public float areaFallThreshold = -5f;

    private void Start()
    {
        manager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.UpdateCheckpoint(point.transform, areaFallThreshold);
        }
    }
}
