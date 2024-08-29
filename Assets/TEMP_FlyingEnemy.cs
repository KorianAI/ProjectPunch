using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class TEMP_FlyingEnemy : MonoBehaviour
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
        railCam.gameObject.SetActive(false);
        rail.transform.position = railStartPos.position;
        pad.transform.position = padStartPos.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) && ps.GetComponent<TargetCams>().currentTarget == transform)
        {
            Defeated();
        }
    }

    public void Defeated()
    {
        railCam.gameObject.SetActive(true);
        if (rail != null && railEndPos != null) { rail.transform.DOMove(railEndPos.transform.position, 2f).OnComplete(ResetCams); }
        if (pad != null && padEndPos != null) { pad.transform.DOMove(padEndPos.transform.position, 2f).OnComplete(ResetCams); }
    }

    void ResetCams()
    {
        railCam.gameObject.SetActive(false);
        ps.tl.freeLook.Priority = 10;
        ps.tl.targetCam.Priority = 1;
        ps.tl.ResetTarget();
        ps.cam.canRotate = true;
    }
}
