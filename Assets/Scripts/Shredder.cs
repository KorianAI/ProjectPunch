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

    float timer;
    float maxTime = 3f;
    bool addToTimer;

    void Start()
    {      
        Extend();
    }

    private void LateUpdate()
    {
        if (addToTimer)
        {
            timer += Time.deltaTime;

            if (timer >= maxTime)
            {
                addToTimer = false;
                timer = 0f;

                Extend();
            }
        }
    }


    public void Pull(PlayerStateManager player)
    {
        addToTimer = false;
        timer = 0f;

        transform.DOKill(false);

        if (invert)
        {
            transform.DOLocalMoveY(retractedPos, pulledSpeed).OnComplete(Retract);
        }
        else
        {
            transform.DOLocalMoveY(extendedPos, pulledSpeed).OnComplete(Retract);
        }
    }

    public void Push(PlayerStateManager player)
    {
        //stop its movement for a few moments

        transform.DOKill(false);

        timer = 0f;
        addToTimer = true;
    }

    void Extend()
    {
        transform.DOKill(false);

        transform.DOLocalMoveY(extendedPos, transitionSpeed).OnComplete(Retract)
            .SetEase(Ease.Linear);
    }

    void Retract()
    {
        transform.DOKill(false);

        transform.DOLocalMoveY(retractedPos, transitionSpeed).OnComplete(Extend)
            .SetEase(Ease.Linear);
    }
}
