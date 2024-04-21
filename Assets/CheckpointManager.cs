using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject defaultStartPoint;
    

    public void UpdateCheckpoint(Transform pos)
    {
        respawnPoint = pos;
    }
}
