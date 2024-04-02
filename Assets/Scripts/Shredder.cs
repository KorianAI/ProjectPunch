using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shredder : MonoBehaviour, IMagnetisable
{
    [Header("Player")]
    public GameObject playerObj;
    public PlayerStateManager ps;

    [Header("Tweening")]
    public float retractedPos;
    public float extendedPos;

    [Space]
    public float transitionSpeed;
    public float pulledSpeed;

    public bool invert;

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
            transform.DOMoveY(retractedPos, pulledSpeed).OnComplete(Retract);
        }
        else
        {
            transform.DOMoveY(extendedPos, pulledSpeed).OnComplete(Retract);
        }
    }

    public void Push(PlayerStateManager player)
    {
        Debug.Log("nuh huh *noise pls*");
    }

    void Extend()
    {
        transform.DOKill(false);

        transform.DOMoveY(extendedPos, transitionSpeed).OnComplete(Retract)
            .SetEase(Ease.Linear);
    }

    void Retract()
    {
        transform.DOKill(false);

        transform.DOMoveY(retractedPos, transitionSpeed).OnComplete(Extend)
            .SetEase(Ease.Linear);
    }
}
