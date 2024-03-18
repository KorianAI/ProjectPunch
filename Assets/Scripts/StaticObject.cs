using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StaticObject : MonoBehaviour, IMagnetisable
{
    public GameObject playerObj;
    public GameObject playerCam;
    public Transform pullPos;

    private void Start()
    {
        playerObj = GameObject.Find("Player");
        playerCam = GameObject.Find("ThirdPersonCamera");
    }

    public void Pull(PlayerStateManager player)
    {
        playerObj.transform.DOMove(pullPos.transform.position, 1f);
        //playerCam.transform.DOMove(pullPos.transform.position, 1f);
    }

    public void Push(PlayerStateManager player)
    {
        Debug.Log("nuh huh");
    }
}
