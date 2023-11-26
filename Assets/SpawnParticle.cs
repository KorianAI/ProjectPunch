using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticle : MonoBehaviour
{
    public ParticleSystem particle;
    public bool canSpawn;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canSpawn)
        {
            var em = particle.emission;
            var dur = particle.duration;

            em.enabled = true;
            particle.Play();

            canSpawn= false;
            Invoke("TurnOff", dur);

        }
    }

    void TurnOff()
    {
        var em = particle.emission;
        em.enabled = false;
        canSpawn = true;
    }
}
