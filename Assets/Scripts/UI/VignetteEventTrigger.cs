using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VignetteEventTrigger : MonoBehaviour
{
    [SerializeField] Respawn respawn;

    public void FadeIn() //called by event trigger on fade out anim
    {
        if (respawn != null)
        {
            Debug.Log("event fading out");

            respawn.FadeIn();
        }
    }

    public void ResetCams() //called by event trigger on fade in anim
    {
        if (respawn != null)
        {
            Debug.Log("event fading in");

            respawn.ResetCams();
        }
    }
}
