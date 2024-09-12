using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShaderWarmup : MonoBehaviour
{
    public float time = .2f;
    public List<VisualEffect> vfx;

    void Start()
    {
        foreach (VisualEffect ve in vfx)
        {
            ve.Play();
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
    }
}
