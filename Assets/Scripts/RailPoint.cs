using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;
using UnityEngine.VFX;

[RequireComponent(typeof(Targetable))]
public class RailPoint : MonoBehaviour, IMagnetisable
{
    public Transform endPosition;
    public Vector3 startPosition;
    public Transform movePos;
    PlayerStateManager ps;

    public float railSpeed;
    public float rotationDur;


    public GameObject nextRail;

    public SplineComputer flipSpline;

    public Tween pullTween;
    public Targetable t;

    public int railPos;

    public bool doSplineAnyway;

    private void Start()
    {
        t = GetComponent<Targetable>();
        t.environment = true;
        startPosition = movePos.position;
    }

    public void Pull(PlayerStateManager player)
    {
        ps = player;
        ps.resources.invincible = true;

        ps.speedboost.Play();
        ps.electricityEffect.Play();

        ps.SwitchState(new PlayerRailState());

        pullTween = ps.transform.DOMove(movePos.position, ps.moveToRailSpeed).OnComplete(SetParent); //pull to the EM
        StartCoroutine(RotatePlayer());

        ps.GetComponent<TargetCams>().maxTime = 2f;
        ps.GetComponent<TargetCams>().StartTimer();
    }

    private void DetermineCamera()
    {
        if (railPos == 1) // middle
        {
            PlayerCameraManager.instance.SwitchNonPlayerCam(PlayerCameraManager.instance.centerRail);
        }

        else if (railPos == 2) // left
        {
            PlayerCameraManager.instance.SwitchNonPlayerCam(PlayerCameraManager.instance.leftRail);
        }

        else if (railPos == 3) // right
        {
            PlayerCameraManager.instance.SwitchNonPlayerCam(PlayerCameraManager.instance.rightRail);
        }
    }

    IEnumerator RotatePlayer()
    {
        while (pullTween.Elapsed() < pullTween.Duration() * .6f)
        {
            yield return null;
        }

        DetermineCamera();
        Quaternion targetRotation = Quaternion.LookRotation(movePos.forward);
        ps.playerObj.transform.DORotateQuaternion(targetRotation, pullTween.Duration() * .4f);
    }

    void SetParent()
    {
        ps.transform.SetParent(movePos); //set parent to EM

        ps.resources.invincible = false;

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

        ps.speedboost.Stop();
        ps.electricityEffect.Stop();

        if (nextRail != null)
        {
            ps.tl.AssignTarget(nextRail.transform, nextRail.GetComponent<Targetable>().targetPoint, 2, true) ;
            ps.splineFollower.enabled = true;
            ps.splineFollower.spline = flipSpline;
            ps.splineFollower.Restart();

            ps.playerObj.DOLookAt(nextRail.transform.position, 0.5f, AxisConstraint.None, Vector3.up);
            ps.ltPressAnim.Play();
            ps.SwitchState(new PlayerRailExit());
        }

        if (doSplineAnyway)
        {
            ps.splineFollower.enabled = true;
            ps.splineFollower.spline = flipSpline;
            ps.splineFollower.Restart();
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
