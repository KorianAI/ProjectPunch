using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StaticObject : MonoBehaviour, IMagnetisable
{
    public GameObject playerObj;
    public Transform pullPos;

    private void Start()
    {
        playerObj = GameObject.Find("Player");
    }

    public void Pull(PlayerStateManager player)
    {
        playerObj.transform.DOMove(pullPos.transform.position, 1.5f);
        playerObj.GetComponent<TargetLock>().currentTarget = null;
        playerObj.GetComponent<TargetLock>().isTargeting = false;
    }

    public void Push(PlayerStateManager player)
    {
        //Should play an audio effect to indicate that this does not work on static objects
        Debug.Log("nuh huh");
    }
}
