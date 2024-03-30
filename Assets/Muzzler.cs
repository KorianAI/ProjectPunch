using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzler : EnemyInfo
{

    public override void Attack(Transform target)
    {
        //agent.SetDestination(transform.position);
        
        if (canAttack)
        {
            //transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            anim.SetTrigger("Attack");
            canAttack = false;
            StartCoroutine(ResetAttack());
        }
       
        IEnumerator ResetAttack()
        {
            yield return new WaitForSeconds(stats.attackSpeed);
            canAttack = true;
            ai.permissionToAttack = false;
            ai.manager.chosenEnemy= null;

            if (ai.manager != null)
            {
                ai.manager.StartAI();
            }
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

    public override void Stunned(Transform player)
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

    }
}
