using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public CinemachineVirtualCamera tCam;        // Target camera (e.g., for lock-on)
    public CinemachineFreeLook playerCam;        // Player camera (FreeLook)

    // Shake related timers and values
    public float shakeTimer;
    public float shakeTimerPlayer;
    public float fovChangeTimer;
    public float fovChangeTimerPlayer;

    // Original FOV values
    public float originalFov;        // For tCam
    public float originalFovPlayer;  // For playerCam

    // Shake and FOV properties
    public float shakeIntensity;
    public float shakeIntensityPlayer;
    public float shakeTimerTotal;
    public float maxShakeIntensity;
    public float fovChangeIntensity;
    public float playerFovChangeIntensity;
    public float fovChangeTimerTotal;
    public float minFOVChange;
    public float maxFOVChange;

    public static CinemachineShake Instance;

    private void Awake()
    {
        tCam = GetComponent<CinemachineVirtualCamera>();

        if (tCam != null)
        {
            originalFov = tCam.m_Lens.FieldOfView;
        }

        if (playerCam != null)
        {
            originalFovPlayer = playerCam.m_Lens.FieldOfView;
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        // Shake for tCam
        if (tCam != null)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = tCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            shakeTimer = time;
            shakeTimerTotal = time;
            shakeIntensity = intensity;
            if (shakeIntensity > maxShakeIntensity)
            {
                shakeIntensity = maxShakeIntensity;
            }
        }

        // Shake for playerCam
        if (playerCam != null)
        {
            CinemachineBasicMultiChannelPerlin playerCamPerlin = playerCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); // Middle rig for shake
            playerCamPerlin.m_AmplitudeGain = intensity;
            shakeTimerPlayer = time;
            shakeTimerTotal = time;
            shakeIntensityPlayer = intensity;
            if (shakeIntensity > maxShakeIntensity)
            {
                shakeIntensityPlayer = maxShakeIntensity;
            }
        }
    }

    public void ChangeFov(float newFov, float time)
    {
        Debug.Log(newFov + " " + time);

        // FOV change for tCam
        if (tCam != null)
        {
            // Instead of adding newFov, directly set the desired value
            tCam.m_Lens.FieldOfView = Mathf.Clamp(originalFov + newFov, minFOVChange, maxFOVChange);
            fovChangeTimer = time;
            fovChangeTimerTotal = time;
            fovChangeIntensity = tCam.m_Lens.FieldOfView; // Set this to the target FOV
            Debug.Log("fovchangeintensity" + " " + fovChangeIntensity);
        }

        // FOV change for playerCam
        if (playerCam != null)
        {
            // Similarly, directly set the desired value for playerCam
            playerCam.m_Lens.FieldOfView = Mathf.Clamp(originalFovPlayer + newFov, minFOVChange, maxFOVChange);
            fovChangeTimerPlayer = time;
            fovChangeTimerTotal = time;
            playerFovChangeIntensity = playerCam.m_Lens.FieldOfView; // Set this to the target FOV
            Debug.Log("player" + " " + playerFovChangeIntensity);
        }
    }

    private void Update()
    {
        //  camera shake decay for tCam
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                tCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(shakeIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }

        // camera shake decay for playerCam
        if (shakeTimerPlayer > 0)
        {
            shakeTimerPlayer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin playerCamPerlin =
                playerCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            playerCamPerlin.m_AmplitudeGain =
                Mathf.Lerp(shakeIntensityPlayer, 0f, 1 - (shakeTimerPlayer / shakeTimerTotal));
        }

        // FOV change decay for tCam
        if (fovChangeTimer > 0)
        {
            fovChangeTimer -= Time.deltaTime;

            tCam.m_Lens.FieldOfView =
                Mathf.Lerp(fovChangeIntensity, originalFov, 1 - (fovChangeTimer / fovChangeTimerTotal));
        }

        // FOV change decay for playerCam
        if (fovChangeTimerPlayer > 0)
        {
            fovChangeTimerPlayer -= Time.deltaTime;

            playerCam.m_Lens.FieldOfView =
                Mathf.Lerp(playerFovChangeIntensity, originalFovPlayer, 1 - (fovChangeTimerPlayer / fovChangeTimerTotal));
        }
    }
}
