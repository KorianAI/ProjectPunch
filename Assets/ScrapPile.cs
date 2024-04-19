using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScrapPile : MonoBehaviour, IDamageable
{
    public GameObject player;

    bool takenDamage;

    public bool canSpawn;

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    public ParticleSystem scrapParticle;

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

            SpawnParticle();
            transform.DOShakeScale(.4f, .5f, 10, 90);

            if (currentHealth <= 0)
            {
                //turn off target lock
                player.GetComponent<TargetLock>().currentTarget = null;
                player.GetComponent<TargetLock>().isTargeting = false;
                player.GetComponent<TargetLock>().lastTargetTag = null;

                //destroy with a plume of extra particles & more scrap to collision
                SpawnParticle();
                SpawnParticle();

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

    private void SpawnParticle()
    {
        if (canSpawn)
        {
            var em = scrapParticle.emission;
            var dur = scrapParticle.main.duration;

            em.enabled = true;
            scrapParticle.Play();

            canSpawn = false;
            Invoke("TurnOff", dur / 2);
            Invoke("UpdatePlayerScrap", .2f);
        }
    }

    private void UpdatePlayerScrap()
    {
        player.GetComponent<PlayerResources>().UpdateScrap(10);
    }

    void TurnOff()
    {
        var em = scrapParticle.emission;
        em.enabled = false;
        canSpawn = true;
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }

    #endregion
}
