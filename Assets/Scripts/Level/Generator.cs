using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class Generator : MonoBehaviour, IDamageable
{
    public GameObject player;
    
    bool takenDamage;  

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;
    public SlotManager healthBar;

    public Animation blastDoorOpen;
    public CinemachineVirtualCamera blastDoorCam;

    public ParticleSystem scrapParticle;
    public ParticleSystem smokeParticle;

    public VolumetricFire[] fires;
    public Light[] lights;

    public EMRail[] rails;

    HealthBars healthBars;

    public AudioSource source;
    public AudioClip takeDamage;
    public AudioClip doors;


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        StartCoroutine(OpenDoor());
    //    }
    //}

    private void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.currentValue = currentHealth;

        healthBars = GetComponentInChildren<HealthBars>();

        source = GetComponent<AudioSource>();
    }

    #region health

    public void TakeDamage(float damage)
    {
        if (!takenDamage)
        {
            healthBars.ShowBarsAttacked();

            takenDamage = true;
            currentHealth -= damage;
            healthBar.currentValue = currentHealth;
            healthBar.DrawSlots();

            transform.DOShakeScale(.2f, .1f, 10, 90);

            source.PlayOneShot(takeDamage);

            if (currentHealth <= 0)
            {
                Break();
            }

            else
            {
                StartCoroutine(ResetTakenDamage());
            }
        }

        IEnumerator ResetTakenDamage()
        {
            yield return new WaitForSeconds(.2f);
            takenDamage = false;
        }
    }

    private void Break()
    {
        foreach (EMRail rail in rails) //pauses all EM rails, ensuring they remain in sync
        {
            rail.MoveToNextPoint();
            rail.source.loop = true;
            rail.source.clip = rail.active;
            rail.source.Play();
        }

        StartCoroutine(OpenDoor());
        
        healthBars.HideBars();

        smokeParticle.Play();

        foreach (VolumetricFire fire in fires)
        {
            fire.enabled = true;
        }

        foreach (Light light in lights)
        {
            light.enabled = true;
        }

        source.Stop();
        source.PlayOneShot(doors);

        player.GetComponent<TargetLock>().currentTarget = null;
        player.GetComponent<TargetLock>().isTargeting = false;
        player.GetComponent<TargetLock>().lastTargetTag = null;
    }

    #endregion

    public IEnumerator OpenDoor()
    {
        //blast door cam
        CameraManager.SwitchNonPlayerCam(blastDoorCam);
        blastDoorOpen.Play();
        yield return new WaitForSecondsRealtime(3);

        //return to collision cam
        CameraManager.SwitchPlayerCam(PlayerStateManager.instance.playerCam);
    }
}
