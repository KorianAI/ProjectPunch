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

    public bool canBeHit = true;

    public Animator vignette;

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
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            TakeDamage(10000);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!canBeHit) { return; }
       

        boss.anim.SetTrigger("Hit");

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

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        foreach (GameObject u in boss.ui)
        {
            u.SetActive(false);
        }

        boss.enabled = false;
        vignette.Play("EndScreen");

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

        transform.DOMove(player.pullPosition.position, 1f).OnComplete(() =>
        {
            player.canAttack = true;
        });
        DOTween.To(() => player.playerCam.m_Lens.FieldOfView, x => player.playerCam.m_Lens.FieldOfView = x, 50, .25f);
    }

    public void Push(PlayerStateManager player)
    {
        //
    }
}