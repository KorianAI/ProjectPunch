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
    public HealthBar healthBar;

    public Animation blastDoorOpen;
    public CinemachineVirtualCamera blastDoorCam;

    public ParticleSystem scrapParticle;
    public ParticleSystem smokeParticle;

    public EMRail[] rails;

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
        }

        StartCoroutine(OpenDoor());

        healthBar.gameObject.SetActive(false);

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
