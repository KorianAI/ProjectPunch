using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{

    [SerializeField] ParticleSystem dustParticle;


    public void DustPlay()
    {
        dustParticle.Play();
    }
}
