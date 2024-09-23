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

    public Transform forwardDirection;

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
            PlayerCameraManager.instance.SwitchNonPlayerCam(PlayerCameraManager.instance.padCam);

            if (nextTarget != null)
            {
                PlayerCameraManager.instance.padCam.m_Lens.FieldOfView = 60;
                Vector3 lookAtPos = new Vector3(nextTarget.transform.position.x, ps.playerObj.position.y, nextTarget.transform.position.z);
                ps.playerObj.DOLookAt(lookAtPos, 0f);
                StartCoroutine(NextTarget(player));
            }

            else
            {
                Vector3 lookAtPos = new Vector3(forwardDirection.transform.position.x, ps.playerObj.position.y, forwardDirection.transform.position.z);
                ps.playerObj.DOLookAt(lookAtPos, 0f);
                PlayerCameraManager.instance.padCam.m_Lens.FieldOfView = 90;
            }

            bounceVFX.Play();

            player.SwitchState(new PlayerBounceState());
            player.tl.ResetTarget();





            player.splineFollower.enabled = true;
            player.splineFollower.spline = flipSpline;

            player.splineFollower.Restart();
        }
    }

    private IEnumerator NextTarget(PlayerStateManager player)
    {
        yield return new WaitForSeconds(player.nextRailLockDelay);
        PlayerCameraManager.instance.SwitchNonPlayerCam(PlayerCameraManager.instance.targetCam);
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
