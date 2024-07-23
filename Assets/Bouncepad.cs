using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;

public class Bouncepad : MonoBehaviour, IMagnetisable
{
    public SplineComputer flipSpline;
    CharacterController cc;
    public bool playerInCollider;
    public float duration = 1f;

    private void Start()
    {
        cc = PlayerStateManager.instance.GetComponent<CharacterController>();
    }

    public void Pull(PlayerStateManager player)
    {
        
    }

    public void Push(PlayerStateManager player)
    {
        if (playerInCollider)
        {
            player.splineFollower.followDuration = duration;
            player.splineFollower.enabled = true;
            player.splineFollower.spline = flipSpline;

            player.SwitchState(new PlayerBounceState());
            
            player.splineFollower.Restart();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == cc)
        {
            playerInCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == cc)
        {
            playerInCollider = false;
        }
    }
}
