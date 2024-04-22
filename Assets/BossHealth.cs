using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class BossHealth : MonoBehaviour, IDamageable
{
    public BossInfo boss;
    public EnemySO stats;
    public float currentHealth;
    public float currentArmour;
    public bool hasArmour;

    [Header("UI")]
    public SlotManager healthBar;
    public SlotManager armourBar;
    public Animator uiAnim;

    public GameObject shatterVFX;

    private void Start()
    {
        currentHealth = stats.health;
        healthBar.currentValue = currentHealth;
        healthBar.maxValue = stats.health;
        hasArmour = true;

        currentArmour = stats.armour;
        armourBar.currentValue = currentArmour;
        armourBar.maxValue = stats.armour;
      
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && hasArmour) { TakeDamage(100); }
        if (Input.GetMouseButtonDown(1) && !hasArmour) { RegainArmour(); }
    }

    public void TakeDamage(float damage)
    {
        if (hasArmour)
        {
            currentArmour -= damage;
            armourBar.currentValue = currentArmour;
            armourBar.DrawSlots();
            if (currentArmour <= 0)
            {
                boss.Stunned();
                Instantiate(shatterVFX, transform.position, Quaternion.identity);
                hasArmour = false;
                uiAnim.Play("ArmourRemove");
            }
        }

        else
        {
            currentHealth -= damage;
            healthBar.currentValue = currentHealth;
            healthBar.DrawSlots();
        }
    }

    public void RegainArmour()
    {
        hasArmour = true;
        currentArmour = stats.armour;
        armourBar.currentValue = currentArmour;
        armourBar.DrawSlots();
        uiAnim.Play("ArmourRegen");
    }
}
