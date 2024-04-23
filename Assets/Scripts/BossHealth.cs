using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class BossHealth : MonoBehaviour, IDamageable, IMagnetisable
{
    public Cashmere boss;
    public EnemySO stats;
    public float currentHealth;
    public float currentArmour;
    public bool hasArmour;

    public float successiveHits;
    Coroutine resetHitCount;

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

    public void TakeDamage(float damage)
    {
        //damage *= 0.5f;
        if (! boss.stunned) {
            if (resetHitCount != null) { StopCoroutine(resetHitCount); }
            resetHitCount = StartCoroutine(SuccessiveHits());
            successiveHits++;
        }
        

        if (successiveHits >= 3 && boss.playerOnSpotlight)
        {
            boss.needsToAtk2= true;
            boss.SelectNextAttack();
        }

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

    IEnumerator SuccessiveHits()
    {
        yield return new WaitForSeconds(2);
        successiveHits = 0;
    }

    public void RegainArmour()
    {
        hasArmour = true;
        currentArmour = stats.armour;
        armourBar.currentValue = currentArmour;
        armourBar.DrawSlots();
        uiAnim.Play("ArmourRegen");
    }

    public void Pull(PlayerStateManager player)
    {
        if (hasArmour) { return; }

        transform.DOMove(player.pullPosition.position, 1f);
        DOTween.To(() => player.playerCam.m_Lens.FieldOfView, x => player.playerCam.m_Lens.FieldOfView = x, 50, .25f);
    }

    public void Push(PlayerStateManager player)
    {
        //
    }
}
