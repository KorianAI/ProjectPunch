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

    public CinemachineVirtualCamera railCam;
    public CinemachineVirtualCamera targetCam;
    public CinemachineFreeLook playerCam;

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
        ps.transform.DOMove(movePos.position, 1f).OnComplete(SetParent); //pull to the EM

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
        PlayerCameraManager.instance.SwitchPlayerCam();


        ps.cam.canRotate = true;

        ps.transform.SetParent(null);
        ps.SwitchState(new PlayerAirState());

        ps.anim.Play("PlayerInAir");
        ps.anim.SetBool("onRail", false);
        movePos.position = startPosition;
    }
}
