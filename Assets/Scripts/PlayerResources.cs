using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;



[DefaultExecutionOrder(0)]
public class PlayerResources : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float currentHealth;
    public float maxHealth;
    public bool invincible;

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
    public ShiftInfo shift;

    //shifts
    public bool scrapShift;

    [SerializeField] CinemachineFreeLook scrapCam;
    [SerializeField] CinemachineFreeLook regularCam;


    public float testDMG;

    public SlotManager healthBar;
    public SlotManager armourBar;

    public static event Action enterScrapStyle;
    public static event Action exitScrapStyle;

    public static event Action enterScrapShift;
    public static event Action exitScrapShift;

    public InputActionReference scrapShiftKeybind;

    [SerializeField] PlayerStateManager stateManager;

    PlayerAudioManager audioManager;

    private void OnEnable()
    {
        CameraManager.RegisterPC(scrapCam);
        CameraManager.RegisterPC(regularCam);
        scrapShiftKeybind.action.performed += ScrapShiftKeybind;
    }

    private void OnDisable()
    {
        CameraManager.UnRegisterPC(scrapCam);
        CameraManager.UnRegisterPC(regularCam);
        scrapShiftKeybind.action.performed -= ScrapShiftKeybind;
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

        audioManager = GetComponent<PlayerAudioManager>();
    }

    private void Update()
    {
        if (currentScrap > 0 && scrapDecrease && (scrapStyle || scrapShift))
        {
            scrapDecreaseTimer += Time.deltaTime;
            if (scrapDecreaseTimer >= .01)
            {
                if (scrapShift)
                {
                    UpdateScrap(scrapDecreaseAmnt * 1.5f);
                }

                else
                {
                    UpdateScrap(scrapDecreaseAmnt);
                }
               
                scrapDecreaseTimer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            UpdateScrap(100);
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            TakeDamage(10);
        }
    }

    private void ScrapShiftKeybind(InputAction.CallbackContext obj)
    {
        // style to shift
        if (currentScrap == maxScrap && !scrapShift)
        {
            ActivateScrapStyle(false);
            ActivateScrapShift(true);
        }

        else
        {
            // from shift to style
            if (scrapShift && currentScrap > 0)
            {
                ActivateScrapShift(false);
                ActivateScrapStyle(true);
            }

            // nothing to style
            else if (!scrapStyle && currentScrap > 50)
            {
                CameraManager.SwitchPlayerCam(scrapCam);
                ActivateScrapStyle(true);
            }

            // style to nothing
            else
            {
                CameraManager.SwitchPlayerCam(regularCam);
                ActivateScrapStyle(false);
            }

        }
    }

    private void ActivateScrapStyle(bool on)
    {
        if (on) { scrapStyle = true; } else if (!on) { scrapStyle = false; }

        if (scrapStyle)
        {          
            enterScrapStyle?.Invoke();
        }

        else
        {           
            exitScrapStyle?.Invoke();
        }

    }

    private void ActivateScrapShift(bool on)
    {
        if (on) { scrapShift = true; } else if (!on) { scrapShift = false; }

        if (scrapShift)
        {
            enterScrapShift?.Invoke();
        }

        else
        {
            exitScrapShift?.Invoke();
        }
    }

    public void TakeDamage(float damage)
    {
        if (invincible) return;

        audioManager.BaseAttack();

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
            if (currentHealth > 10)
            {
                currentHealth -= damage;
                healthBar.currentValue = currentHealth;
                healthBar.DrawSlots();
            }

            else if (currentHealth <= 10)
            {
                currentHealth -= damage;
                healthBar.currentValue = currentHealth;
                healthBar.DrawSlots(); 
                GetComponent<Respawn>().ResetPlayer();
            }
        }
    }

    public void UpdateScrap(float amount)
    {
        if (amount > 0 )
        {
            scrapDecrease = false;
            StopAllCoroutines();
            StartCoroutine("ResetScrapDecrease");
            audioManager.CollectScrap();
        }
        currentScrap += amount;
        if (currentScrap + amount > maxScrap)
        {
            currentScrap = maxScrap;
        }
        if (currentScrap +amount < 0)
        {
            currentScrap = 0;
            if (scrapShift)
            {
                ActivateScrapShift(false);
            }
            if (scrapStyle)
            {
                ActivateScrapStyle(false);
            }

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
        audioManager.ArmourRestore();
    }

    public void ReplenishAll()
    {
        currentHealth = maxHealth;
        currentArmour = maxArmour;
        healthBar.currentValue = currentHealth;
        healthBar.DrawSlots();
        armourBar.currentValue = currentArmour;
        armourBar.DrawSlots();
        currentScrap = 0;
    }
}
