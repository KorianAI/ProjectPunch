using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Transform respawnPoint;
    //public Transform defaultStartPoint;
    

    public void UpdateCheckpoint(Transform pos)
    {
        respawnPoint = pos;
    }
}
