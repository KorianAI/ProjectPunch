using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;
using Cinemachine;

public class EMRail : MonoBehaviour, IMagnetisable
{
    public GameObject playerObj;
    public PlayerStateManager ps;
    public Transform pullPos;

    public bool playOnStart;

    [Header("Points")]
    public Vector3[] points;
    public int arrayPosition;
    public float transitionspeed;

    [Header("Rotations")]
    public Vector3[] rotations;
    public float rotationSpeed;

    [Header("Connected Rails")]
    public EMRail[] rails;

    [Header("Cameras")]
    public CinemachineFreeLook playerCam;
    public CinemachineVirtualCamera railCam;

    private void Start()
    {
        if (playOnStart)
        {
            MoveToNextPoint();
        }
    }

    public void Pull(PlayerStateManager player)
    {
        foreach (EMRail script in rails) //pauses all EM rails, ensuring they remain in sync
        {
            script.transform.DOPause();
        }

        ps = player;
        ps.rail = this;
        ps.speedlines.SetActive(true);

        player.SwitchState(player.railState);
        playerObj.transform.DOMove(pullPos.transform.position, 0.5f).OnComplete(SetParent); //pull to the EM
        DOTween.To(() => player.playerCam.m_Lens.FieldOfView, x => player.playerCam.m_Lens.FieldOfView = x, 70, .1f);
    }

    void SetParent()
    {
        playerObj.transform.SetParent(pullPos.transform); //set parent to EM
        ps.speedlines.SetActive(false);
        DOTween.To(() => ps.playerCam.m_Lens.FieldOfView, x => ps.playerCam.m_Lens.FieldOfView = x, 50, .25f);

        CameraManager.SwitchNonPlayerCam(ps.railCam);
        ps.playerObj.forward = ps.rail.gameObject.transform.forward;

        foreach (EMRail script in rails) //plays all EM rails
        {
            script.transform.DOPlay();
        }

        playerObj.GetComponent<TargetLock>().currentTarget = null;
        playerObj.GetComponent<TargetLock>().isTargeting = false;
        ps.anim.Play("Hang");
        ps.anim.SetBool("onRail", true);
        ps.speedlines.SetActive(false);
    }

    public void Push(PlayerStateManager player)
    {
        Debug.Log("nuh huh *noise pls*");
    }

    public void MoveToNextPoint()
    {
        transform.DOMove(points[arrayPosition], transitionspeed).OnComplete(RotateAtNextPoint)
            .SetEase(Ease.Linear);
        
        if (arrayPosition < (points.Length -1))
        {
            arrayPosition++;
        }
        else 
        {
            arrayPosition = 0;
        }
    }

    private void RotateAtNextPoint()
    {
        transform.DORotate(rotations[arrayPosition], rotationSpeed).OnComplete(MoveToNextPoint)
            .SetEase(Ease.Linear);
    }
}
