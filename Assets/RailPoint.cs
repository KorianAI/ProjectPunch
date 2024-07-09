using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;

public class RailPoint : MonoBehaviour, IMagnetisable
{
    public Transform endPosition;
    public Vector3 startPosition;
    public Transform movePos;
    PlayerStateManager ps;

    public float railSpeed;
    public float rotationDur;

    public CinemachineVirtualCamera railCam;
    public CinemachineVirtualCamera targetCam;
    public CinemachineFreeLook playerCam;

    public GameObject nextRail;

    public SplineComputer flipSpline;

    public Tween pullTween;

    private void Start()
    {
        startPosition = movePos.position;
    }

    public void Pull(PlayerStateManager player)
    {
        ps = player;
        ps.resources.invincible = true;

        PlayerCameraManager.instance.SwitchNonPlayerCam(PlayerCameraManager.instance.railCam);
        ps.SwitchState(new PlayerRailState());

        pullTween = ps.transform.DOMove(movePos.position, 1f).OnComplete(SetParent); //pull to the EM
        StartCoroutine(RotatePlayer());
    }

    IEnumerator RotatePlayer()
    {
        while (pullTween.Elapsed() < pullTween.Duration() * .8f)
        {
            yield return null;
        }
        
        Quaternion targetRotation = Quaternion.LookRotation(movePos.forward);
        ps.playerObj.transform.DORotateQuaternion(targetRotation, pullTween.Duration() * .2f);
    }

    void SetParent()
    {
        ps.transform.SetParent(movePos); //set parent to EM

        ps.speedlines.SetActive(false);

        ps.resources.invincible = false;
        ps.canAttack = true;


        ps.anim.Play("Hang");
        ps.anim.SetBool("onRail", true);
        MoveAlongRail();
        ps.tl.ResetTarget();
    }

    void MoveAlongRail()
    {

        movePos.DOMove(endPosition.position, railSpeed).SetEase(Ease.Linear).OnComplete(Detach);
    }

    public void Push(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }

    void Detach()
    {
        ps.transform.SetParent(null);

        if (nextRail != null)
        {
            ps.tl.AssignTarget(nextRail.transform, nextRail.GetComponent<Targetable>().targetPoint, 2);
            ps.splineFollower.enabled = true;
            ps.splineFollower.spline = flipSpline;
            ps.splineFollower.Restart();

            ps.playerObj.DOLookAt(nextRail.transform.position, 0.5f, AxisConstraint.None, Vector3.up);
            ps.SwitchState(new PlayerRailExit());
        }

        else
        {
            ps.cam.canRotate = true;
            PlayerCameraManager.instance.SwitchPlayerCam();
            ps.SwitchState(new PlayerAirState());
            ps.anim.SetBool("onRail", false);
            ps.anim.Play("PlayerInAir");
        }


        movePos.position = startPosition;
    }
}
