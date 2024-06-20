using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSFX : MonoBehaviour
{
    AudioSource source;
    public AudioClip clip;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySFX()
    {
        source.PlayOneShot(clip);
    }
}
