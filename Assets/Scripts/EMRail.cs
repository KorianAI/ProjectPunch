using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EMRail : MonoBehaviour, IMagnetisable
{
    public GameObject playerObj;
    public Transform pullPos;

    public Vector3[] points;
    public int arrayPosition;
    public float transitionspeed;

    private void Start()
    {
        MoveToNextPoint();
    }

    public void Pull(PlayerStateManager player)
    {
        transform.DOPause();
        playerObj.transform.DOMove(pullPos.transform.position, 1.5f).OnComplete(SetParent); //pull to
        //player.applyGrav = false;
    }

    void SetParent()
    {
        playerObj.transform.SetParent(pullPos.transform); //set parent to EM
        transform.DOPlay();
    }

    public void Push(PlayerStateManager player)
    {
        if (playerObj.GetComponent<TargetLock>().currentTarget != null)
        {
            playerObj.GetComponent<TargetLock>().currentTarget = null;
            playerObj.GetComponent<TargetLock>().isTargeting = false;

            //unset parent
            playerObj.transform.SetParent(null);

            //player.applyGrav = true; //add gravity multipler
            //change anim
        }
        else
        {
            //Should play an audio effect to indicate that this does not work on static objects
            Debug.Log("nuh huh");
        }
    }


    private void MoveToNextPoint()
    {
        transform.DOMove(points[arrayPosition], transitionspeed).OnComplete(MoveToNextPoint);
        
        if (arrayPosition < (points.Length -1))
        {
            arrayPosition++;
        }
        else 
        {
            arrayPosition = 0;
        }
    }
}
