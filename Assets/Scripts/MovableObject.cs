using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovableObject : MonoBehaviour, IMagnetisable
{
    public GameObject player;
    public PlayerStateManager ps;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void Pull(PlayerStateManager player)
    {
        ps = player;
        transform.DOMove(player.pullPosition.position, 1f);
        //transform.DOShakeRotation(1, 15f, 10, 90);
    }

    public void Push(PlayerStateManager player)
    {
        transform.DOMove(transform.position + player.playerObj.forward * player.kbForce, 1f);
        //transform.DOShakeRotation(1, 15f, 10, 90);
    }
}
