using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour, IMagnetisable
{
    Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void Pull(PlayerStateManager player)
    {
        anim.Play();
    }

    public void Push(PlayerStateManager player)
    {
        
    }
}
