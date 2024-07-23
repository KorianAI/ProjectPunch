using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;

public class Bouncepad : MonoBehaviour, IMagnetisable
{
    PlayerStateManager ps;
    public SplineComputer flipSpline;

    public void Pull(PlayerStateManager player)
    {
        
    }

    public void Push(PlayerStateManager player)
    {
        //player.pm.JumpForce();

        player.splineFollower.enabled = true;
        player.splineFollower.spline = flipSpline;
        player.splineFollower.Restart();

        //speed lines, pull out cam?
    }

    //if player is standing on the pad and presses push
    //activate the spline follower
    //make the player move along the curve of the spline
    
}
