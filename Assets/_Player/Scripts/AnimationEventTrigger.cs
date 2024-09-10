using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{

    [SerializeField] ParticleSystem dustParticle;
    [SerializeField] Nailgun nailgun;
    [SerializeField] RealSteel rs;

    public void DustPlay()
    {
        dustParticle.Play();
    }

    public void NailBurst()
    {
        nailgun.Shoot();
    }

    public void ConcentratedNail()
    {
        nailgun.ConcentratedNail();
    }

    public void PunchBlast()
    {
        rs.PunchBlast();
    }
}
