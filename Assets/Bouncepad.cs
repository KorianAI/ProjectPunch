using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;

public class Bouncepad : MonoBehaviour, IMagnetisable
{
    public SplineComputer flipSpline;
    public CapsuleCollider td;
    public bool playerInCollider;
    public float duration;

    public void Pull(PlayerStateManager player)
    {
        
    }

    public void Push(PlayerStateManager player)
    {
        //if player is standing on the pad and presses push
        //activate the spline follower
        //make the player move along the curve of the spline


        //player.pm.JumpForce();

        if (playerInCollider)
        {
            player.splineFollower.followDuration = duration;
            player.splineFollower.enabled = true;
            player.splineFollower.spline = flipSpline;
            player.splineFollower.Restart();

            //speed lines, pull out cam?
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInCollider = true;

        //if (other == td)
        //{
            
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        playerInCollider = false;
        //if (other == td)
        //{
            
        //}
    }
}
