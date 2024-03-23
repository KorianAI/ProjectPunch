using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Non-Player Cameras")]
    public List<CinemachineVirtualCamera> cameras;
    public CinemachineVirtualCamera activeCamera;
    public static CameraManager instance;

    [Header("Player Cameras")]
    public List<CinemachineFreeLook> playerCameras;
    public CinemachineFreeLook activePlayerCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SwitchNonPlayerCam (CinemachineVirtualCamera cam)
    {
        //if (activeCamera = cam) return;

        foreach (CinemachineFreeLook c in playerCameras)
        {
            c.Priority = 0;
        }

        foreach (CinemachineVirtualCamera c in cameras)
        {
            if (c == cam)
            {
                c.Priority = 1;
                activeCamera = cam;
            }

            else
            {
                c.Priority = 0;
            }
        }
    }

    public void SwitchPlayerCam(CinemachineFreeLook cam)
    {
        //if (activePlayerCamera = cam) return
        Debug.Log("called switch");

        foreach (CinemachineVirtualCamera c in cameras)
        {
            c.Priority = 0;
            Debug.Log(c);
        }

        foreach (CinemachineFreeLook c in playerCameras)
        {
            if (c == cam)
            {
                c.Priority = 1;
                activePlayerCamera = cam;
                Debug.Log(c);
            }

            else
            {
                c.Priority = 0;
                Debug.Log(c);
            }
        }
    }
}
