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
    PlayerStateManager ps;

    public bool playerInCollider;
    public float duration = 1f;

    public GameObject nextRail;

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
            player.SwitchState(new PlayerBounceState());
            player.tl.ResetTarget();

            if (nextRail != null)
            {
                player.tl.AssignTarget(nextRail.transform, nextRail.GetComponent<Targetable>().targetPoint, 2, true);
                player.ltPressAnim.Play();
            }

            player.splineFollower.enabled = true;
            player.splineFollower.spline = flipSpline;

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
