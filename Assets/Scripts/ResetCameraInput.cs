using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCameraInput : MonoBehaviour
{
    public CinemachineBrain brain;
    CinemachineInputProvider provider;

    private void Start()
    {
        provider = GetComponent<CinemachineInputProvider>();
    }

    public void ResetInput()
    {
        StartCoroutine(WaitForTransition());
    }

    IEnumerator WaitForTransition()
    {
        Debug.Log("dude");
        yield return new WaitForSeconds(brain.ActiveBlend.Duration);
        provider.enabled = true;
    }
}
