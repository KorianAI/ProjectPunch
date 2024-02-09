using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public ParticleSystem particle;
    public bool canSpawn;
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Owwww u hit: " + this.gameObject);
        SpawnParticle();
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
}
