using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    CinemachineVirtualCamera brain;
    public float shakeTimer;
    public float fovChangeTimer;
    public float originalFov;

    public float shakeIntensity;
    public float shakeTimerTotal;
    public float maxShakeIntensity;

    public float fovChangeIntensity;
    public float fovChangeTimerTotal;
    public float maxFOVChange;

    public static CinemachineShake Instance;

    private void Awake()
    {
        brain = GetComponent<CinemachineVirtualCamera>();
        if (Instance == null )
        {
            Instance = this;
            originalFov = brain.m_Lens.FieldOfView;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = brain.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
        shakeIntensity = intensity;
        if (shakeIntensity > maxShakeIntensity)
        {
            shakeIntensity = maxShakeIntensity;
        }
    }

    public void ChangeFov(float newFov, float time)
    {
        brain.m_Lens.FieldOfView += newFov;
        if (brain.m_Lens.FieldOfView > maxFOVChange)
        {
            brain.m_Lens.FieldOfView = maxFOVChange;
        }
        fovChangeTimer = time;
        fovChangeTimerTotal = time;
        fovChangeIntensity = brain.m_Lens.FieldOfView += newFov;
    }

    private void Update()
    {


        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                brain.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
            Mathf.Lerp(shakeIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
           
        }

        if (fovChangeTimer > 0)
        {
            fovChangeTimer -= Time.deltaTime;

                brain.m_Lens.FieldOfView = 
                Mathf.Lerp(fovChangeIntensity, originalFov, 1 - (fovChangeTimer / fovChangeTimerTotal));
            
        }
    }
}
