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

    public bool playerInCollider;
    public float duration = 1f;

    public GameObject nextTarget;

    public VisualEffect bounceVFX;

    private void Start()
    {
        cc = PlayerStateManager.instance.GetComponent<CharacterController>();
    }

    public void Pull(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }

    public void Push(PlayerStateManager player)
    {
        if (playerInCollider)
        {
            bounceVFX.Play();

            player.SwitchState(new PlayerBounceState());
            player.tl.ResetTarget();

            if (nextTarget != null)
            {
                StartCoroutine (NextTarget(player));
            }
            else
            {
                Debug.Log("no next target");
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
