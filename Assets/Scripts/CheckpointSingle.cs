using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    CheckpointManager manager;
    public GameObject point;

    private void Start()
    {
        manager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint" + name);
            manager.UpdateCheckpoint(point.transform);
        }
    }
}
