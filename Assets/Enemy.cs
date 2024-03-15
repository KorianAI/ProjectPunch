using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour, IDamageable, IMagnetisable
{
    public ParticleSystem particle;
    public bool canSpawn;
    public GameObject player;

    public PlayerStateManager ps;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Owwww u hit: " + this.gameObject);
        SpawnParticle();
        transform.DOShakeScale(1, .1f, 10, 90);
    }

    private void SpawnParticle()
    {
        if (canSpawn)
        {
            var em = particle.emission;
            var dur = particle.main.duration;

            em.enabled = true;
            particle.Play();

            canSpawn = false;
            Invoke("TurnOff", dur /2);
            Invoke("UpdatePlayerScrap", .2f);
        }
    }

    private void UpdatePlayerScrap()
    {
        player.GetComponent<PlayerResources>().UpdateScrap(10);
    }

    void TurnOff()
    {
        var em = particle.emission;
        em.enabled = false;
        canSpawn = true;
        
    }

    public void Pull(PlayerStateManager player)
    {
        ps = player;
        transform.DOMove(player.pullPosition.position, 1f).onComplete = PlayerReset;   
    }

    public void PlayerReset()
    {
        ps.canAttack = true;
        ps.SwitchState(ps.moveState);   
    }

    public void Push(PlayerStateManager player)
    {
        transform.DOMove(transform.position + player.playerObj.forward * player.kbForce, 1f);
    }
}
