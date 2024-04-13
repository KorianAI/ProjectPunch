using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[DefaultExecutionOrder(0)]
public class PlayerResources : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float currentHealth;
    public float maxHealth;

    [Header("Armour")]
    public bool hasArmour;
    public float currentArmour;
    public float maxArmour;

    [Header("Scrap")]
    public float currentScrap;
    public float maxScrap;
    public Image scrapFillImage;
    public float scrapDecreaseAmnt;
    float scrapDecreaseTimer;
    public bool scrapDecrease;

    public bool scrapStyle;
    public StyleInfo lightStyle;
    public StyleInfo heavyStyle;

    [SerializeField] CinemachineFreeLook scrapCam;
    [SerializeField] CinemachineFreeLook regularCam;


    public float testDMG;

    public HealthBar healthBar;

    private void OnEnable()
    {
        CameraManager.RegisterPC(scrapCam);
        CameraManager.RegisterPC(regularCam);
    }

    private void OnDisable()
    {
        CameraManager.UnRegisterPC(scrapCam);
        CameraManager.UnRegisterPC(regularCam);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        //currentScrap = maxScrap;

        healthBar.maxHealth = maxHealth;
        healthBar.currentHealth = currentHealth;
    }

    private void Update()
    {
        if (currentScrap > 0 && scrapDecrease && scrapStyle)
        {
            scrapDecreaseTimer += Time.deltaTime;
            if (scrapDecreaseTimer >= .01)
            {
                UpdateScrap(scrapDecreaseAmnt);
                scrapDecreaseTimer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ActivateScrapStyle();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(testDMG);
        }
    }

    private void ActivateScrapStyle()
    {
        scrapStyle = !scrapStyle;

        if (scrapStyle)
        {
            CameraManager.SwitchPlayerCam(scrapCam);
        }

        else
        {
            CameraManager.SwitchPlayerCam(regularCam);
        }

    }

    public void TakeDamage(float damage)
    {
       
        currentHealth -= damage;
        healthBar.currentHealth = currentHealth;
        healthBar.DrawSlots();
        //UpdateHealthUI();
    }

    //void UpdateHealthUI()
    //{
    //    healthFillImage.fillAmount = currentHealth / maxHealth;
    //}

    public void UpdateScrap(float amount)
    {
        if (amount >0 )
        {
            scrapDecrease = false;
            StopAllCoroutines();
            StartCoroutine("ResetScrapDecrease");
        }
        currentScrap += amount;

        if (currentScrap + amount > maxScrap)
        {
            currentScrap = maxScrap;
        }
        if (currentScrap +amount < 0)
        {
            currentScrap = 0;
        }

        UpdateScrapUI();
    }

    void UpdateScrapUI()
    {
        scrapFillImage.fillAmount = currentScrap / maxScrap;
    }

    private IEnumerator ResetScrapDecrease()
    {
        yield return new WaitForSeconds(1.5f);
        scrapDecrease = true;
    }

}
