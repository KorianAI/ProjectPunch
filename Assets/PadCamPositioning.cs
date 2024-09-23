using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadCamPositioning : MonoBehaviour
{
    public Transform pos;

    private void Update()
    {
        transform.position = pos.position;
        transform.rotation = pos.rotation;
    }
}
