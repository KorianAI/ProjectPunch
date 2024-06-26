using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager instance;

    public Gamepad gamePad;

    private Coroutine stopRumbleAfterTimeCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void RumblePulse(float lowFreq, float highFreq, float dur)
    {
        gamePad = Gamepad.current;

        if (gamePad != null)
        {
            
            gamePad.SetMotorSpeeds(lowFreq, highFreq);

            stopRumbleAfterTimeCoroutine = StartCoroutine(StopRumble(dur, gamePad));
        }
    }

    private IEnumerator StopRumble(float dur, Gamepad pad)
    {
        float elapsedTime = 0f;
        while (elapsedTime < dur)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gamePad.SetMotorSpeeds(0f, 0f);

    }
}
