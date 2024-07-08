using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public static class CameraManager
{
    [Header("Non-Player Cameras")]
    static List<CinemachineVirtualCamera> nonPlayerCameras = new List<CinemachineVirtualCamera>();
    public static CinemachineVirtualCamera activeCamera = null;

    [Header("Player Cameras")]
    static List<CinemachineFreeLook> playerCameras = new List<CinemachineFreeLook>();
    public static CinemachineFreeLook activePlayerCamera = null;


    #region Register
    public static void RegisterVC(CinemachineVirtualCamera vCam)
    {
        nonPlayerCameras.Add(vCam);
    }

    public static void UnRegisterVC(CinemachineVirtualCamera vCam)
    {
        nonPlayerCameras.Remove(vCam);
    }

    public static void RegisterPC(CinemachineFreeLook pCam)
    {
        playerCameras.Add(pCam);
    }

    public static void UnRegisterPC(CinemachineFreeLook pCam)
    {
        playerCameras.Remove(pCam);
    }
    #endregion

    

    public static void SwitchNonPlayerCam (CinemachineVirtualCamera cam)
    {
        cam.Priority = 10;
        activeCamera = cam;

        foreach (CinemachineFreeLook c in playerCameras)
        {
            c.Priority = 0;
        }

        foreach (CinemachineVirtualCamera c in nonPlayerCameras)
        {
            if (c == !cam && c.Priority != 0)
            {
                c.Priority = 0;
                
            }
        }
    }

    public static void SwitchPlayerCam(CinemachineFreeLook cam)
    {
        
        cam.Priority = 10;
        activePlayerCamera = cam;


        foreach (CinemachineVirtualCamera c in nonPlayerCameras)
        {
            c.Priority = 0;
        }

        foreach (CinemachineFreeLook c in playerCameras)
        {
            if (c != cam)
            {
                c.Priority = 0;
            }

            //CinemachineInputProvider input = c.GetComponent<CinemachineInputProvider>();
            //if (input != null)
            //{
            //    input.enabled = false;
            //    ResetCameraInput r = c.GetComponent<ResetCameraInput>();
               
            //}
        }

    }
}
