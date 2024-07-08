using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

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
        RotatePlayer();
        ps.transform.DOMove(movePos.position, 1f).OnComplete(SetParent); //pull to the EM

    }

    void RotatePlayer()
    {
        Quaternion targetRotation = Quaternion.LookRotation(movePos.forward);
        ps.playerObj.transform.DORotateQuaternion(targetRotation, rotationDur);
    }

    void SetParent()
    {
        ps.transform.SetParent(movePos); //set parent to EM
        ps.playerObj.transform.forward = movePos.forward;
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
        if (nextRail != null)
        {
            ps.tl.AssignTarget(nextRail.transform, nextRail.GetComponent<Targetable>().targetPoint, 2);
            //ps.playerObj.transform.DORotate(nextRail.transform, .5f);
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

        ps.transform.SetParent(null);
        movePos.position = startPosition;
    }
}
