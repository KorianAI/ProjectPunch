using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ActivateRailMovement : MonoBehaviour
{
    [Space]
    public GameObject rail;
    public Transform railStartPos;
    public Transform railEndPos;

    [Space]

    public GameObject pad;
    public Transform padStartPos;
    public Transform padEndPos;

    [Space]

    public CinemachineVirtualCamera railCam;

    PlayerStateManager ps;

    private void Start()
    {
        ps = PlayerStateManager.instance;
        if (railCam != null)
        {
            railCam.gameObject.SetActive(false);
        }
        rail.transform.position = railStartPos.position;
        pad.transform.position = padStartPos.position;
    }

    public void Defeated()
    {
        if (rail != null && railEndPos != null) { rail.transform.DOMove(railEndPos.transform.position, 2f); }
        if (pad != null && padEndPos != null) { pad.transform.DOMove(padEndPos.transform.position, 2f); }
    }

    public void ResetCams()
    {
        railCam.gameObject.SetActive(false);
        ps.tl.freeLook.Priority = 10;
        ps.tl.targetCam.Priority = 1;
        ps.tl.ResetTarget();
        ps.cam.canRotate = true;
    }
}
