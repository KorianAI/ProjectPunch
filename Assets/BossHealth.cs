using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class BossHealth : MonoBehaviour, IDamageable
{
    public EnemySO stats;
    public float currentHealth;
    public float currentArmour;

    [Header("HealthBar")]
    public SlotManager healthBar;
    public SlotManager armourBar;

    private void Start()
    {
        currentHealth = stats.health;
        currentArmour = stats.armour;
        armourBar.currentValue = currentArmour;
        armourBar.maxValue = stats.armour;
        healthBar.currentValue = currentHealth;
        healthBar.maxValue = stats.health;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.currentValue = currentHealth;
        healthBar.DrawSlots();
    }
}
