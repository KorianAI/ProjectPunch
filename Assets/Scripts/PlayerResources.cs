using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    public GameObject armourShatter;

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

    public SlotManager healthBar;
    public SlotManager armourBar;

    public static event Action enterScrapStyle;
    public static event Action exitScrapStyle;

    [SerializeField] PlayerStateManager stateManager;

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
        currentArmour = maxArmour;
        //currentScrap = maxScrap;

        healthBar.maxValue = maxHealth;
        healthBar.currentValue = currentHealth;
        armourBar.maxValue = maxArmour;
        armourBar.currentValue = currentArmour;
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
            enterScrapStyle?.Invoke();
            stateManager.whipAnim.gameObject.SetActive(true);
        }

        else
        {
            CameraManager.SwitchPlayerCam(regularCam);
            exitScrapStyle?.Invoke();
            stateManager.whipAnim.gameObject.SetActive(false);
        }

    }

    public void TakeDamage(float damage)
    {
        if (hasArmour)
        {
            float remainingDamage = damage - currentArmour;
            currentArmour -= damage;
            armourBar.currentValue = currentArmour;
            armourBar.DrawSlots();

            if (currentArmour <= 0)
            {
                hasArmour = false;
                Instantiate(armourShatter, transform.position, Quaternion.identity);

                if (remainingDamage > 0)
                {               
                    currentHealth -= remainingDamage;
                    healthBar.currentValue = currentHealth;
                    healthBar.DrawSlots();
                }
            }
        }

        else
        {
            currentHealth -= damage;
            healthBar.currentValue = currentHealth;
            healthBar.DrawSlots();
        }
    }

    //void UpdateHealthUI()
    //{
    //    healthFillImage.fillAmount = currentValue / maxValue;
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

    private void OnTriggerEnter(Collider other)
    {
        ICollectible collectible = other.GetComponent<ICollectible>();
        if (collectible != null)
        {
            collectible.Collect(this);
        }
    }

    public void ReplenishArmour()
    {
        hasArmour = true;
        currentArmour = maxArmour;
        armourBar.currentValue = currentArmour;
        armourBar.DrawSlots();
    }

}
