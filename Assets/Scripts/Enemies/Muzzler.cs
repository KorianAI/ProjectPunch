using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzler : EnemyInfo
{

    private void Start()
    {
        anim.speed = (agent.speed / stats.moveSpeed) + .25f;
    }

    public override void Attack(Transform target)
    {
        //agent.SetDestination(transform.position);
        
        if (canAttack)
        {
            //transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            Debug.Log("dude");
            canAttack = false;
            Vector3 storedPos = CalculateAttackPosition(target.position);
            ai.transform.DOMove(storedPos, 1).onComplete = AttackAnim;
        }
       

       
    }

    private Vector3 CalculateAttackPosition(Vector3 playerPosition)
    {
        // Calculate the enemy from the enemy to the collision
        Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

        // Calculate the attack position based on the enemy's current position, collision's position, and desired distance

        Vector3 pos = playerPosition - directionToPlayer * stats.range;
        pos.y = ai.transform.position.y;
        return pos;
    }

    void AttackAnim()
    {  
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(stats.attackSpeed);
        canAttack = true;
        ai.attackToken = false;
        ai.manager.chosenEnemy = null;

        if (ai.manager != null)
        {
            ai.manager.StartAI();
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

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        // Calculate the attack position
        Vector3 playerPosition = ai.playerPos.transform.position;// Get collision position
        Vector3 directionToPlayer = (playerPosition - transform.position).normalized;
        Vector3 attackPosition = playerPosition - directionToPlayer * stats.range;

        // Draw a gizmo at the attack position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPosition, 0.1f);
    }
}
