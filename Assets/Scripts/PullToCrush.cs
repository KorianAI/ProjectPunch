using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PullToCrush : MonoBehaviour, IMagnetisable
{
    public Vector3 startPos;
    public Vector3 retractedPos;
    public Vector3 extendedPos;

    public float transitionspeed;
    public float pulledSpeed;

    [Header("Connected Items")]
    public PullToCrush[] objs;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos;

        Extend();
        Debug.Log("extend called");
    }

    public void Pull(PlayerStateManager player)
    {
        Debug.Log("pulled");

        //transform.DOKill(false);

        transform.DOMove(retractedPos, transitionspeed).OnComplete(Retract);
    }

    public void Push(PlayerStateManager player)
    {
        Debug.Log("nuh huh *noise pls*");
    }

    void Extend()
    {
        //foreach (PullToCrush script in objs) //ensures they remain in sync
        //{

        //}
        Debug.Log("extending");

        //transform.DOKill(false);

        transform.DOMove(retractedPos, transitionspeed).OnComplete(Retract);
    }

    void Retract()
    {
        //foreach (PullToCrush script in objs) //ensures they remain in sync
        //{

        //}
        Debug.Log("retracting");

        //transform.DOKill(false);

        transform.DOMove(retractedPos, transitionspeed).OnComplete(Extend);
    }

    
}
