using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    //audioManager = GetComponent<EnemyAudioManager>();
    //EnemyAudioManager audioManager;


    AudioSource source;
    AudioClip currentClip;
    [SerializeField] AudioClip armourRestore;
    [SerializeField] AudioClip baseAttack;
    [SerializeField] AudioClip baseSwing;

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
}
