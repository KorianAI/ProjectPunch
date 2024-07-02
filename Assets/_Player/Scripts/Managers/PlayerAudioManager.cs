using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerAudioManager : MonoBehaviour
{
    public static PlayerAudioManager instance;
    //audioManager = GetComponent<PlayerAudioManager>();

    AudioSource source;
    AudioClip currentClip;

    [SerializeField] AudioClip armourRestore;
    [SerializeField] AudioClip baseAttack;
    [SerializeField] AudioClip baseSwing;
    [SerializeField] AudioClip baseAttackMetallic;
    [SerializeField] AudioClip extendoLight;
    [SerializeField] AudioClip slamSFX;
    [SerializeField] AudioClip shockwave;
    [SerializeField] AudioClip pull;
    [SerializeField] AudioClip push;
    [SerializeField] AudioClip collectScrap;
    [SerializeField] AudioClip[] shotgunHeavy;
    [SerializeField] AudioClip slamExplode;
    [SerializeField] AudioClip shiftSwitch;
    [SerializeField] AudioClip styleSwitch;

    private void Awake()
    {
       if (instance == null)
            {
                instance = this;
            }

       else
        {
            Destroy(this.gameObject);
        }
    }

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
        source.pitch = Random.Range(.9f, 1.1f);
        PlayChosen();
    }



    public void SlamSFX()
    {
        currentClip = slamSFX;
        PlayChosen();
    }

    public void Shockwave()
    {
        currentClip = shockwave;
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

    public void ShiftSwitch()
    {
        currentClip = shiftSwitch;
        PlayChosen();
    }

    public void StyleSwitch()
    {
        currentClip = styleSwitch;
        PlayChosen();
    }
}
