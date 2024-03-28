using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzler : EnemyInfo
{
    public override void Attack(Transform target)
    {
        agent.SetDestination(transform.position);

        if (canAttack)
        {
            anim.SetTrigger("Attack");
            canAttack = false;
            StartCoroutine(ResetAttack());
        }
       
        IEnumerator ResetAttack()
        {
            yield return new WaitForSeconds(stats.attackSpeed);
            canAttack = true;
        }
       
    }

    public override void Chase(Transform destination)
    {
        agent.SetDestination(destination.position);
    }

    public override void Dead()
    {
        

    }


    public override void Idle()
    {
        anim.SetBool("Walking", false);

    }

    public override void Stunned()
    {
        

    }
}
