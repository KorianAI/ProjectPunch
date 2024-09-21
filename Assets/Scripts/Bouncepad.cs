using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;
using UnityEngine.VFX;

public class Bouncepad : MonoBehaviour, IMagnetisable
{
    public SplineComputer flipSpline;
    CharacterController cc;
    PlayerStateManager ps;
    Targetable t;

    public float duration = 1f;
    public float camLockDuration = 1.5f;

    public GameObject nextTarget;

    public VisualEffect bounceVFX;

    private void Start()
    {
        cc = PlayerStateManager.instance.GetComponent<CharacterController>();
        ps = PlayerStateManager.instance;
        t = GetComponent<Targetable>();

        t.pushMe = true;
    }

    public void Pull(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }

    public void Push(PlayerStateManager player)
    {
        if (ps.inBounceCollider)
        {
            bounceVFX.Play();

            player.SwitchState(new PlayerBounceState());
            player.tl.ResetTarget();

            if (nextTarget != null)
            {
                StartCoroutine (NextTarget(player));
            }

            player.splineFollower.enabled = true;
            player.splineFollower.spline = flipSpline;

            player.splineFollower.Restart();
        }
    }

    private IEnumerator NextTarget(PlayerStateManager player)
    {
        yield return new WaitForSeconds(player.nextRailLockDelay);
        player.tl.AssignTarget(nextTarget.transform, nextTarget.GetComponent<Targetable>().targetPoint, 2, true);
        player.ltPressAnim.Play();

        ps.GetComponent<TargetCams>().maxTime = camLockDuration;
        ps.GetComponent<TargetCams>().StartTimer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == cc && t)
        {
            ps.inBounceCollider = true;
            ps.currentPad = this;
            t.SetColor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == cc && t)
        {
            ps.inBounceCollider = false;
            ps.currentPad = null;
            t.ResetColor();
        }
    }
}
