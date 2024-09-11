using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;


[DefaultExecutionOrder(0)]
public class PlayerResources : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float currentHealth;
    public float maxHealth;
    public bool invincible;
    public float dmgShakeAmnt;
    public float dmgShakeDur;

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

    //shifts
    public bool scrapShift;
    public WeaponInfo shift;
    public WeaponInfo mode;
    public WeaponInfo attachment;

    [SerializeField] CinemachineFreeLook scrapCam;
    [SerializeField] CinemachineFreeLook regularCam;

    public Animator scrapAnim;
    public GameObject scrapAnimObj;

    public float testDMG;

    public SlotManager healthBar;
    public SlotManager armourBar;

    public InputActionReference scrapShiftKeybind;

    [SerializeField] PlayerStateManager stateManager;

    PlayerAudioManager audioManager;

    public bool superInvincible;

    public GameObject[] gauntletsObj;
    public GameObject[] bandagesObj;
    public GameObject[] shiftObj;

    public ParticleSystem[] shiftEquipVFX;
    public ParticleSystem[] gauntletEquipVFX;

    [SerializeField] Volume pp;
    [SerializeField] AnimationCurve abbCurve;
    private float abbIntensityLastTime;
    private ChromaticAberration abb;

    private void OnEnable()
    {
        CameraManager.RegisterPC(scrapCam);
        CameraManager.RegisterPC(regularCam);
        scrapShiftKeybind.action.performed += ScrapShiftKeybind;
        CameraManager.SwitchPlayerCam(regularCam);
        pp.profile.TryGet(out abb);
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
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            superInvincible = !superInvincible;
        }

        float abbIntensity = abbCurve.Evaluate(Time.realtimeSinceStartup - abbIntensityLastTime);
        abb.intensity.value = abbIntensity;
    }

    private void ShowIndicator()
    {
        // style to shift
        if (currentScrap == maxScrap && !scrapShift)
        {
            scrapAnimObj.SetActive(true);
            scrapAnim.Play("SSIndicator");
        }
        else
        {
            scrapAnimObj.SetActive(false);
        }
        
    }

    private void ScrapShiftKeybind(InputAction.CallbackContext obj)
    {
        // style to shift
        if (currentScrap == maxScrap && !scrapShift)
        {
            ActivateScrapShift(true);
            scrapAnimObj.SetActive(false);
        }

        else if (scrapShift)
        {
            ActivateScrapShift(false);
        }
    }

    private void ActivateScrapShift(bool on)
    {
        if (on) 
        { 
            scrapShift = true; 
            ChangeGauntlets(2); 
        } 
        
        else if (!on)        
        { 
            scrapShift = false;
            ChangeGauntlets(3);
        }



    }

    public void TakeDamage(float damage)
    {
        if (invincible || superInvincible) return;

        if (hasArmour)
        {
            float remainingDamage = damage - currentArmour;
            currentArmour -= damage;
            armourBar.currentValue = currentArmour;
            armourBar.DrawSlots();
            DamageEffect();

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
            float healthRemaining = currentHealth - damage;

            if (healthRemaining >= 1)
            {
                currentHealth -= damage;
                healthBar.currentValue = currentHealth;
                DamageEffect();
                healthBar.DrawSlots();
            }

            else if (healthRemaining <= 0)
            {
                currentHealth -= damage;
                healthBar.currentValue = currentHealth;
                healthBar.DrawSlots();
                DamageEffect();
                StartCoroutine(Die());
            }
        }

        CinemachineShake.Instance.ShakeCamera(dmgShakeAmnt, dmgShakeDur);
    }

    public IEnumerator Die()
    {
        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Null); //prevent movement
        stateManager.anim.SetTrigger("Die");
        yield return new WaitForSeconds(1);
        GetComponent<Respawn>().ResetPlayer();
        yield return new WaitForSeconds(1);
        stateManager.anim.SetTrigger("Get Up");
        yield return new WaitForSeconds(2.5f);
        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Player); //allow movement
    }

    public void UpdateScrap(float amount)
    {
        if (amount > 0 )
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
            if (scrapShift)
            {
                ActivateScrapShift(false);
            }

        }

        UpdateScrapUI();
        ShowIndicator();
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

    public void ChangeGauntlets(int type)
    {
        switch (type)
        {
            case 1: // Regular > Mode
                SetActive(bandagesObj, true);
                SetActive(gauntletsObj, true);
                SetActive(shiftObj, false);
                foreach (ParticleSystem p in gauntletEquipVFX)
                {
                    var em = p.emission;
                    var dur = p.main.duration;

                    em.enabled = true;
                    p.Play();
                    Invoke("TurnOff", dur / 2);
                }
                break;

            case 2: // Mode > Shift
                SetActive(bandagesObj, false);
                SetActive(gauntletsObj, false);
                SetActive(shiftObj, true);
                foreach (ParticleSystem p in shiftEquipVFX)
                {
                    var em = p.emission;
                    var dur = p.main.duration;

                    em.enabled = true;
                    p.Play();
                    Invoke("TurnOff", dur / 2);
                }
                break;

            case 3: // Shift > Mode
                SetActive(bandagesObj, true);
                SetActive(gauntletsObj, true);
                SetActive(shiftObj, false);
                foreach (ParticleSystem p in gauntletEquipVFX)
                {
                    var em = p.emission;
                    var dur = p.main.duration;

                    em.enabled = true;
                    p.Play();
                    Invoke("TurnOff", dur / 2);
                }
                break;

            case 4: // Any > Gauntlets
                SetActive(bandagesObj, false);
                SetActive(gauntletsObj, true);
                SetActive(shiftObj, false);
                foreach (ParticleSystem p in gauntletEquipVFX)
                {
                    var em = p.emission;
                    var dur = p.main.duration;

                    em.enabled = true;
                    p.Play();
                    Invoke("TurnOff", dur / 2);
                }
                break;
        }
    }

    void TurnOff()
    {
        foreach (ParticleSystem p in gauntletEquipVFX)
        {
            var em = p.emission;
            em.enabled = false;
        }

        foreach (ParticleSystem p in shiftEquipVFX)
        {
            var em = p.emission;
            em.enabled = false;
        }
    }

    private void SetActive(GameObject[] objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(isActive);
        }
    }

    void DamageEffect()
    {
        abbIntensityLastTime = Time.realtimeSinceStartup;
    }
}
