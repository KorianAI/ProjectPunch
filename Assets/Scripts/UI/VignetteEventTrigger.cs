using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VignetteEventTrigger : MonoBehaviour
{
    [SerializeField] Respawn respawn;

    public void RespawnAppear() //called by event trigger on fade out anim
    {
        if (respawn != null)
        {
            respawn.RespawnAppear();
        }
    }

    public void RespawnDisappear() //called by event trigger on fade in anim
    {
        if (respawn != null)
        {
            respawn.RespawnDisappear();
        }
    }
}
