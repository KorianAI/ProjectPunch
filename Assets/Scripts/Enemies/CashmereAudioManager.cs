using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashmereAudioManager : MonoBehaviour
{
    //audioManager = GetComponent<CashmereAudioManager>();
    //CashmereAudioManager audioManager;

    public AudioSource source;
    AudioClip currentClip;

    [SerializeField] AudioClip scrapVolleyBuild;
    [SerializeField] AudioClip scrapVolleyShoot;
    [SerializeField] AudioClip shockwave;
    [SerializeField] AudioClip slamExplosion;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void PlayChosen()
    {
        source.PlayOneShot(currentClip);
    }

    public void ScrapVolleyBuild()
    {
        currentClip = scrapVolleyBuild;
        PlayChosen();
    }
    public void ScrapVolleyShoot()
    {
        currentClip = scrapVolleyShoot;
        PlayChosen();
    }
    public void Shockwave()
    {
        currentClip = shockwave;
        PlayChosen();
    }
    public void SlamExplosion()
    {
        currentClip = slamExplosion;
        PlayChosen();
    }
}
