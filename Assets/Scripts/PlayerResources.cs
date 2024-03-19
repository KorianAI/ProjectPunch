using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResources : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float currentHealth;
    public float maxHealth;
    public Image healthFillImage;

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

    private void Start()
    {
        currentHealth = maxHealth;
        currentScrap = maxScrap;
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
            scrapStyle = !scrapStyle;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthFillImage.fillAmount = currentHealth / maxHealth;
    }

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
