using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScrapPile : MonoBehaviour, IDamageable, ITargeted
{
    public GameObject player;

    bool takenDamage;

    public bool canSpawn;

    AudioSource source;
    public AudioClip damageSound;

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    public ParticleSystem scrapParticle;

    [Header("Material Change")]
    public Material mat;
    public Material mat1;
    public Material mat2;
    public Material mat3;

    public Color colour;

    public MeshRenderer mRenderer1;
    public MeshRenderer mRenderer2;
    public MeshRenderer mRenderer3;
    public MeshRenderer mRenderer4;
    public MeshRenderer mRenderer5;
    public MeshRenderer mRenderer6;

    public float outlineThicknessMultiplier = 2;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("Player");
        source = GetComponent<AudioSource>();
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
            source.PlayOneShot(damageSound);

            if (currentHealth <= 0)
            {
                //turn off target lock
                player.GetComponent<TargetCams>().CancelLock();

                //destroy with a plume of extra particles & more scrap to collision
                SpawnParticle();
                SpawnParticle();

                Destroy();
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
        Destroy(gameObject, .25f);
    }

    #endregion

    public void SetColor()
    {
        Material newMat = Instantiate(mat);

        mRenderer1.material = newMat;
        mRenderer2.material = newMat;
        mRenderer3.material = newMat;
        mRenderer4.material = newMat;
        mRenderer5.material = newMat;
        mRenderer6.material = newMat;

        newMat.SetColor("_OutlineColor", colour);
        newMat.SetFloat("_OutlineWidth", mat.GetFloat("_OutlineWidth") * outlineThicknessMultiplier);
    }

    public void ResetColor()
    {
        mRenderer1.material = mat1;
        mRenderer2.material = mat1;
        mRenderer3.material = mat2;
        mRenderer4.material = mat2;
        mRenderer5.material = mat3;
        mRenderer6.material = mat3;
    }

    public void OnTarget()
    {
        SetColor();
    }

    public void OnTargetLost()
    {
        ResetColor();
    }
}
