using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZCameraShake;
using TreeEditor;
using Cinemachine;

public class Generator : MonoBehaviour, IDamageable
{
    public GameObject player;
    public PlayerStateManager ps;
    
    bool takenDamage;  

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;
    public HealthBar healthBar;

    public Material[] mats;
    bool isFading;

    public Animation blastDoorOpen;
    public CinemachineVirtualCamera blastDoorCam;

    public ParticleSystem scrapParticle;
    public ParticleSystem smokeParticle;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(OpenDoor());
        }
    }

    #region health

    private void Start()
    {
        healthBar.maxHealth = maxHealth;
        healthBar.currentHealth = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!takenDamage)
        {
            takenDamage = true;
            currentHealth -= damage;
            healthBar.currentHealth = currentHealth;
            healthBar.DrawSlots();

            transform.DOShakeScale(.2f, .1f, 10, 90);

            if (currentHealth <= 0)
            {
                Die();
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

    private void Die()
    {
        StartCoroutine(OpenDoor());

        healthBar.gameObject.SetActive(false);

        //smoke particles start
        smokeParticle.Play();
    }

    #endregion

    public IEnumerator OpenDoor()
    {
        //blast door cam
        CameraManager.SwitchNonPlayerCam(blastDoorCam);
        blastDoorOpen.Play();
        yield return new WaitForSecondsRealtime(3);

        //return to player cam
        CameraManager.SwitchPlayerCam(PlayerStateManager.instance.playerCam);
    }
}
