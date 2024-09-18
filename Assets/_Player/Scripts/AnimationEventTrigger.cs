using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AnimationEventTrigger : MonoBehaviour
{

    [SerializeField] GameObject dustParticle;
    [SerializeField] Transform dustStartPos;
    [SerializeField] Nailgun nailgun;
    [SerializeField] RealSteel rs;
    [SerializeField] PlayerResources r;

    public void DustPlay()
    {
        GameObject dust = Instantiate(dustParticle, dustStartPos.transform.position, Quaternion.identity);
        dust.GetComponent<VisualEffect>().Play();
        Destroy(dust, 0.4f);
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

    public void Shift()
    {
        r.ActivateScrapShift(true);
    }

    public void RSNail()
    {
        rs.RSNail();
    }

}
