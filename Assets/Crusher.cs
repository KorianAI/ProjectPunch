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

        transform.DOLocalMoveY(extendedPos, pulledSpeed).OnComplete(Retract);
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

        transform.DOLocalMoveY(extendedPos, downSpeed).OnComplete(Retract)
            .SetEase(Ease.Linear);
    }

    void Retract()
    {
        transform.DOKill(false);

        transform.DOLocalMoveY(retractedPos, upSpeed).OnComplete(Extend)
            .SetEase(Ease.Linear);
    }
}
