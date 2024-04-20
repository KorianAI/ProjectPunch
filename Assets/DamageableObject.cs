using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageableObject : MonoBehaviour, IDamageable
{
    GameObject player;

    bool takenDamage;

    float currentHealth;
    [SerializeField] float maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("Player");
    }

    #region health

    public void TakeDamage(float damage)
    {
        if (!takenDamage)
        {
            takenDamage = true;
            currentHealth -= damage;

            transform.DOShakeScale(.4f, .5f, 10, 90);

            if (currentHealth <= 0)
            {
                //turn off target lock
                player.GetComponent<TargetLock>().currentTarget = null;
                player.GetComponent<TargetLock>().isTargeting = false;
                player.GetComponent<TargetLock>().lastTargetTag = null;

                Invoke("Destroy", .25f);
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

    private void Destroy()
    {
        Destroy(this.gameObject);
    }

    #endregion
}
