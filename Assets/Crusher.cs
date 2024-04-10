using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crusher : MonoBehaviour, IMagnetisable
{
    [Header("Player")]
    public GameObject playerObj;
    public PlayerStateManager ps;

    [Header("Tweening")]
    public float retractedPos;
    public float extendedPos;

    [Space]
    public float upSpeed;
    public float downSpeed;
    public float pulledSpeed;

    public bool invert;

    //[Header("Piston Arm")]
    //public GameObject arm;
    
    //[Space]
    //public float armStartDisplace;
    //public float armExpandDisplace;

    //[Space]
    //public float armStartScale;
    //public float armExpandScale;
    //public float scaleReturnSpeed;

    void Start()
    {
        Extend();
    }

    public void Pull(PlayerStateManager player)
    {
        Debug.Log(gameObject + "pulled");

        transform.DOKill(false);

        if (invert)
        {
            transform.DOLocalMoveY(retractedPos, pulledSpeed).OnComplete(Retract);

            //arm.transform.DOLocalMoveY(armExpandDisplace, pulledSpeed);
            //arm.transform.DOScaleY(armExpandScale, pulledSpeed);
        }
        else
        {
            transform.DOLocalMoveY(extendedPos, pulledSpeed).OnComplete(Retract);

            //arm.transform.DOLocalMoveY(armExpandDisplace, pulledSpeed);
            //arm.transform.DOScaleY(armExpandScale, pulledSpeed);
        }
    }

    public void Push(PlayerStateManager player)
    {
        Debug.Log("nuh huh *noise pls*");
    }

    void Extend()
    {
        transform.DOKill(false);

        transform.DOLocalMoveY(extendedPos, downSpeed).OnComplete(Retract)
            .SetEase(Ease.Linear);

        //arm.transform.DOLocalMoveY(armExpandDisplace, downSpeed);
        //arm.transform.DOScaleY(armExpandScale, downSpeed);
    }

    void Retract()
    {
        transform.DOKill(false);

        transform.DOLocalMoveY(retractedPos, upSpeed).OnComplete(Extend)
            .SetEase(Ease.Linear);

        //arm.transform.DOLocalMoveY(armStartDisplace, upSpeed);
        //arm.transform.DOScaleY(armStartScale, upSpeed);
    }
}
