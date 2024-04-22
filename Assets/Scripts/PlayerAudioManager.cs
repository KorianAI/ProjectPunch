using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    //audioManager = GetComponent<PlayerAudioManager>();
    //PlayerAudioManager audioManager;


    AudioSource source;
    AudioClip currentClip;

    [SerializeField] AudioClip armourRestore;
    [SerializeField] AudioClip baseAttack;
    [SerializeField] AudioClip baseSwing;
    [SerializeField] AudioClip baseAttackMetallic;
    [SerializeField] AudioClip extendo_1_2;
    [SerializeField] AudioClip pull;
    [SerializeField] AudioClip push;
    [SerializeField] AudioClip collectScrap;
    [SerializeField] AudioClip[] shotgunHeavy;
    [SerializeField] AudioClip slamExplode;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void PlayChosen()
    {
        source.PlayOneShot(currentClip);
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

    public void BaseAttackMetallic()
    {
        currentClip = baseAttackMetallic;
        PlayChosen();
    }

    public void Extendo_1_2()
    {
        currentClip = extendo_1_2;
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
        currentClip = shotgunHeavy[Random.Range(0, shotgunHeavy.Length)];
        PlayChosen();
    }

    public void SlamExplode()
    {
        currentClip = slamExplode;
        PlayChosen();
    }
}
