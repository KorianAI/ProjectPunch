using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
    [SerializeField] ExtendoArms extArms;
    [SerializeField] ParticleSystem dustParticle;


    public void TriggerShockwave(float value)
    {
        if (extArms != null)
        {
            extArms.Shockwave(value);
        }
    }

    public void DustPlay()
    {
        dustParticle.Play();
    }
}
