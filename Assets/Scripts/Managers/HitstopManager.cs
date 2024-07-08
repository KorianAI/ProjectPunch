using UnityEngine;
using System.Collections;

public class HitstopManager : MonoBehaviour
{
    public static HitstopManager Instance;

    Coroutine timeStop;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerHitstop(float duration, params GameObject[] objectsToAffect)
    {
        StartCoroutine(HitstopCoroutine(duration, objectsToAffect));
    }

    private IEnumerator HitstopCoroutine(float duration, GameObject[] objectsToAffect)
    {
        foreach (var obj in objectsToAffect)
        {
            if (obj != null)
            {
                // Pause animations
                Animator animator = obj.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.speed = 0;
                }

                // Pause movement (custom method to handle CharacterController)
                CharacterController characterController = obj.GetComponent<CharacterController>();
                if (characterController != null)
                {
                    characterController.enabled = false;
                }
            }
        }

        yield return new WaitForSecondsRealtime(duration);

        foreach (var obj in objectsToAffect)
        {
            if (obj != null)
            {
                // Resume animations
                Animator animator = obj.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.speed = 1;
                }

                // Resume movement (custom method to handle CharacterController)
                CharacterController characterController = obj.GetComponent<CharacterController>();
                if (characterController != null)
                {
                    characterController.enabled = true;
                }
            }
        }
    }

    public void AlterTimeScale(float t, float d)
    {
        if (timeStop != null)
        {
            StopCoroutine(timeStop);
        }

        timeStop = StartCoroutine(ChangeTimeScale(t, d));
    }

    IEnumerator ChangeTimeScale(float target, float duration)
    {
        float start = Time.timeScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ensure smooth transition even if time scale is changing
            Time.timeScale = Mathf.Lerp(start, target, elapsed / duration);
            yield return null; // Wait for the next frame
        }

        Time.timeScale = target; // Ensure the time scale is set to the target value at the end
    }
}
