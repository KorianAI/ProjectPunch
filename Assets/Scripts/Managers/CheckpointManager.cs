using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject defaultStartPoint;
    Respawn respawn; 

    private void Start()
    {
        respawn = GameObject.Find("Player").GetComponent<Respawn>();
    }

    public void UpdateCheckpoint(Transform pos, float threshold)
    {
        respawnPoint = pos;
        respawn.fallThreshold = threshold;
    }
}
