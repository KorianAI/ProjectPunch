using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConcentratedNail : MonoBehaviour, IMagnetisable
{
    public Transform enemy;
    public float speed;
    public bool stuck;
    public float dur;
    private void Start()
    {
        transform.DOMove(enemy.position, dur).OnComplete(PierceTarget);
    }

    private void PierceTarget()
    {
        transform.SetParent(enemy);
    }

    public void Pull(PlayerStateManager player)
    {
        
    }

    public void Push(PlayerStateManager player)
    {
        
    }
}
