using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    AudioSource source;
    AudioClip currentClip;

    [SerializeField] AudioClip armourBreak;
    [SerializeField] AudioClip armourRestore;
    [SerializeField] AudioClip baseAttack;
    [SerializeField] AudioClip baseSwing;
    [SerializeField] AudioClip extendo;
    [SerializeField] AudioClip pull;
    [SerializeField] AudioClip push;
    [SerializeField] AudioClip collectScrap;
    [SerializeField] AudioClip shotgunHeavy;
    [SerializeField] AudioClip slamExplode;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void PlayChosen()
    {
        source.PlayOneShot(currentClip);
    }

    public void ArmourBreak()
    {
        currentClip = armourBreak;
        PlayChosen();
    }

    public void ArmourRestore()
    {
        currentClip = armourRestore;
        PlayChosen();
    }

    public void BaseAttack()
    {
        currentClip = baseAttack;
        PlayChosen();
    }

    public void BaseSwing()
    {
        currentClip = baseSwing;
        PlayChosen();
    }

    public void Extendo()
    {
        currentClip = extendo;
        PlayChosen();
    }

    public void Pull()
    {
        currentClip = pull;
        PlayChosen();
    }

    public void Push()
    {
        currentClip = push;
        PlayChosen();
    }

    public void CollectScrap()
    {
        currentClip = collectScrap;
        PlayChosen();
    }

    public void ShotgunHeavy()
    {
        currentClip = shotgunHeavy;
        PlayChosen();
    }

    public void SlamExplode()
    {
        currentClip = slamExplode;
        PlayChosen();
    }
}
