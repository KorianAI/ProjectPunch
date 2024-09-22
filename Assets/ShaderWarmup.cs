using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShaderWarmup : MonoBehaviour
{
    public float time = .2f;
    public GameObject vfxParent;
    public GameObject particleParent;
    public List<VisualEffect> vfx;
    public List<ParticleSystem> particles;
    public GameObject heatDistortion;

    void Start()
    {
        if (heatDistortion != null)
        {
            heatDistortion.SetActive(true);
        }

        foreach (VisualEffect ve in vfx)
        {
            ve.Play();
        }
        foreach (ParticleSystem pa in particles)
        {
            pa.Play();
        }

        StartCoroutine(End());
    }

    IEnumerator End()
    {
        yield return new WaitForSecondsRealtime (time);

        foreach (VisualEffect ve in vfx)
        {
            ve.Stop();
        }
        foreach (ParticleSystem pa in particles)
        {
            pa.Stop();
        }

        if (vfxParent != null)
        {
            Destroy(vfxParent);
        }
        if (particleParent != null)
        {
            Destroy(particleParent);
        }
    }
}
