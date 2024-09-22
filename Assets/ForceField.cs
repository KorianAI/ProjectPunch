using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ForceField : MonoBehaviour, IDamageable, ITargeted
{
    public GameObject player;
    public GameObject protectedObj;

    [Header("Colour Change")]
    public ParticleSystem vfx1;

    public Color colourDefault;
    public Color colourDamaged;
    public Color colourNoDamage;
    public Color colourTargeted;
    public Color currentColour;

    public float damageTime = 1f;

    private void Start()
    {
        player = GameObject.Find("Player");
        SetColour(colourDefault);
        protectedObj.GetComponent<Collider>().enabled = false;
    }

    #region health

    public void TakeDamage(float damage)
    {
        SetColour(colourNoDamage); //show that it cannot take damage from normal attacks
        StartCoroutine(ResetColourDelay());
        Debug.Log("no damage from normal weapons");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ParryProjectile") && collision.gameObject.GetComponent<TurretProjectile>().parried)
        {
            transform.DOShakeScale(.4f, .5f, 10, 90);
            SetColour(colourDamaged);
            Debug.Log("took parry damage!!!");

            //swap target lock to enemy
            protectedObj.GetComponent<Collider>().enabled = true;
            player.GetComponent<TargetCams>().AssignTarget(protectedObj.transform, protectedObj.GetComponent<Targetable>().targetPoint, 1, true);

            Destroy();
            //sfx?
        }
    }

    private void Destroy()
    {
        Debug.Log("Destroying Field");
        Destroy(gameObject, .25f);
    }

    #endregion

    public void SetColour(Color colour)
    {
        currentColour = colour;
        vfx1.startColor = currentColour;
    }

    IEnumerator ResetColourDelay()
    {
        yield return new WaitForSeconds(damageTime);
        SetColour(currentColour);
    }

    public void OnTarget()
    {
        SetColour(colourTargeted);
    }

    public void OnTargetLost()
    {
        SetColour(colourDefault);
    }
}
