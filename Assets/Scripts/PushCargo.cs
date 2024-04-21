using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PushCargo : MonoBehaviour, IMagnetisable
{
    Animation anim;

    bool played = false;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }
    public void Pull(PlayerStateManager player)
    {
        //transform.DOMove(player.pullPosition.position, 1f);
        transform.DOShakeRotation(.5f, 5f, 10, 90);
    }

    public void Push(PlayerStateManager player)
    {
        //transform.DOMove(transform.position + player.playerObj.forward * player.kbForce, 1f);
        //transform.DOShakeRotation(1, 15f, 10, 90);

        if (!played)
        {
            played = true;

            anim.Play();

            player.GetComponent<TargetLock>().currentTarget = null;
            player.GetComponent<TargetLock>().isTargeting = false;
            player.GetComponent<TargetLock>().lastTargetTag = null;
        }
    }
}
