using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendoArms : ShiftInfo
{

    public GameObject rumbleVFX;
    public GameObject shockwaveVFX;
    public GameObject smallerShockwaveVFX;
    public Transform rumblePosition;

    public float knockbackForce;

    // HEAVY COMBO

    public override void HAttack1(float damage, float range)
    {
        player.anim.runtimeAnimatorController = hAttack1;
    }

    public override void HAttack2(float damage, float range)
    {
        player.anim.runtimeAnimatorController = hAttack2;
    }

    public override void HAttack3(float damage, float range)
    {
        player.anim.runtimeAnimatorController = hAttack3;
    }

    // LIGHT COMBO

    public override void LAttack1(float damage, float range)
    {
        player.anim.runtimeAnimatorController = lAttack1;
    }

    public override void LAttack2(float damage, float range)
    {
        player.anim.runtimeAnimatorController = lAttack2;
    }

    public override void LAttack3(float damage, float range)
    {
        player.anim.runtimeAnimatorController = lAttack3;
    }

    public void Shockwave(float type)
    {        
        if (type == 1)
        {
            // push
            CheckForEnemies(1);
        }

        else if (type == 2)
        {
            // pull
            CheckForEnemies(2);
        }

        else if (type == 4)
        {
            CheckForEnemies(4);
        }

        else
        {
            CheckForEnemies(0);
        }
    }

    private void CheckForEnemies(float type)
    {      
        if (type != 4)
        {
            GameObject shockwaveEffect = Instantiate(shockwaveVFX, rumblePosition.position, Quaternion.Euler(-90, 0, 0));
            GameObject rumbleEffect = Instantiate(rumbleVFX, rumblePosition.position, Quaternion.identity);
            Collider[] enemies = Physics.OverlapSphere(rumblePosition.position, range, player.enemyLayer);
            foreach (Collider c in enemies)
            {
                c.GetComponent<IDamageable>().TakeDamage(atkDmg);
                RumbleManager.instance.RumblePulse(.25f, .4f, .1f);

                if (type == 1)
                {
                    Vector3 directionToCenter = (c.transform.position - rumblePosition.position).normalized;
                    float knockbackDistance = 1f;
                    Vector3 knockbackDestination = c.transform.position + directionToCenter * knockbackDistance;
                    c.transform.DOMove(knockbackDestination, 1f);

                    c.GetComponent<EnemyHealth>().GetStunned(2f);
                }

                else if (type == 2)
                {
                    c.transform.DOMove(rumblePosition.position, 1f);
                }

                else if (type == 0)
                {
                    // idk
                }

                //if (c.GetComponent<EnemyHealth>() != null)
                //{
                //    c.GetComponent<EnemyHealth>().GetStunned(.5f);
                //}
            }
        }

        else
        {
            GameObject shockwaveEffect = Instantiate(smallerShockwaveVFX, rumblePosition.position, Quaternion.Euler(-90, 0, 0));
            Collider[] enemies = Physics.OverlapSphere(rumblePosition.position, range * .8f, player.enemyLayer);
            foreach (Collider c in enemies)
            {
                c.GetComponent<IDamageable>().TakeDamage(atkDmg * .5f);
                RumbleManager.instance.RumblePulse(.25f, .4f, .1f);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rumblePosition.position, range);
    }
}
