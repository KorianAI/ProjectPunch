using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PushCargo : MonoBehaviour, IMagnetisable
{
    Animation anim;

    bool played = false;

    public AudioSource source;
    public AudioClip pushed;

    Targetable targetable;
    public bool canPull = false;
    public bool canPush = true;

    private void Start()
    {
        anim = GetComponent<Animation>();
        source = GetComponent<AudioSource>();
        targetable = GetComponent<Targetable>();
    }

    public void Pull(PlayerStateManager player)
    {
        transform.DOShakeRotation(.5f, 5f, 10, 90);
    }

    public void Push(PlayerStateManager player)
    {
        //transform.DOShakeRotation(1, 15f, 10, 90);

        if (!played)
        {
            played = true;

            anim.Play();

            source.PlayOneShot(pushed);
            player.tl.ResetTarget();
            player.cam.canRotate = true;
            targetable.untargetable = true;
            PlayerCameraManager.instance.SwitchPlayerCam();
        }
    }
}
