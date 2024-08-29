using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class TEMP_FlyingEnemy : MonoBehaviour
{
    public GameObject rail;
    public Transform railStartPos;
    public Transform railEndPos;
    public CinemachineVirtualCamera railCam;

    private void Start()
    {
        railCam.gameObject.SetActive(false);
        rail.transform.position = railStartPos.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Defeated();
        }
    }

    public void Defeated()
    {
        railCam.gameObject.SetActive(true);
        rail.transform.DOMove(railEndPos.transform.position, 2f).OnComplete(ResetCams);
    }

    void ResetCams()
    {
        railCam.gameObject.SetActive(false);
    }
}
