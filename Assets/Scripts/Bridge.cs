using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bridge : MonoBehaviour, IMagnetisable
{
    Animation anim;

    bool played = false;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void Pull(PlayerStateManager player)
    {
        if (!played)
        {
            played = true;

            anim.Play();

            player.GetComponent<TargetLock>().currentTarget = null;
            player.GetComponent<TargetLock>().isTargeting = false;
            player.GetComponent<TargetLock>().lastTargetTag = null;
        }
    }

    public void Push(PlayerStateManager player)
    {
        transform.DOShakeRotation(.5f, 5f, 10, 90);
    }
}
