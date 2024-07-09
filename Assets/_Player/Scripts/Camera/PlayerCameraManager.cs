using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [Header("Freelook")]
    public CinemachineFreeLook thirdPersonCam;
    public CinemachineFreeLook[] freelookCams;


    [Header("Virtual")]
    public CinemachineVirtualCamera targetCam;
    public CinemachineVirtualCamera finisherCam;
    public CinemachineVirtualCamera[] virtualCams;

    [Header("Rails")]
    public CinemachineVirtualCamera centerRail;
    public CinemachineVirtualCamera leftRail;
    public CinemachineVirtualCamera rightRail;

    public CinemachineVirtualCamera activeCam;

    public static PlayerCameraManager instance;

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
        VirtualCamerasArray();
    }

    private void VirtualCamerasArray()
    {
        List<CinemachineVirtualCamera> camList = new List<CinemachineVirtualCamera>();

        // Get all fields of the current class
        FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (FieldInfo field in fields)
        {
            // Check if the field is of type CinemachineVirtualCamera
            if (field.FieldType == typeof(CinemachineVirtualCamera))
            {
                // Get the value of the field (which should be a CinemachineVirtualCamera)
                CinemachineVirtualCamera cam = (CinemachineVirtualCamera)field.GetValue(this);
                if (cam != null)
                {
                    camList.Add(cam);
                }
            }
        }

        // Assign the collected cameras to the virtualCams array
        virtualCams = camList.ToArray();
    }

    public void SwitchNonPlayerCam(CinemachineVirtualCamera cam)
    {
        cam.Priority = 10;
        activeCam = cam;

        thirdPersonCam.Priority = 0;

        foreach (CinemachineVirtualCamera c in virtualCams)
        {
            if (c != cam && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    public void SwitchPlayerCam()
    {
        thirdPersonCam.Priority = 10;

        foreach (CinemachineVirtualCamera c in virtualCams)
        {
            c.Priority = 0;
        }
    }
}
