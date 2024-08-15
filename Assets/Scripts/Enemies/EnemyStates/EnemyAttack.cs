using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    public bool strafing;

    bool playerLeft;

    public override void EnterState(EnemyAI enemyAI)
    {
        base.EnterState(enemyAI);
        enemyAI.available = true;
        enemyAI.enemy.anim.SetBool("Patrolling", true);
        if (enemyAI.circle != null) { enemyAI.StopCoroutine(enemyAI.circle); }        
       
    }

    public override void ExitState(EnemyAI enemyAI)
    {
        base.ExitState(enemyAI);
        enemyAI.enemy.anim.SetBool("Patrolling", false);
    }


    public override void FrameUpdate(EnemyAI enemyAI)
    {
        base.FrameUpdate(enemyAI);
        enemyAI.transform.LookAt(new Vector3(enemyAI.playerPos.transform.position.x, enemyAI.transform.position.y, enemyAI.playerPos.transform.position.z));

        if (enemyAI.attackToken)
        {
            
            enemyAI.agent.SetDestination(enemyAI.transform.position);

            if (enemyAI.circle != null)
            {             
                enemyAI.StopCoroutine(enemyAI.circle);
            }        
            enemyAI.enemy.Attack(enemyAI.playerPos.transform);
            enemyAI.rePositioning = false;

            //enemyAI.audioManager.BaseSwing();
        }

        else
        {
            //if (!enemyAI.rePositioning && enemyAI.manager.chosenEnemy != null)
            //{
            //    if (enemyAI.circle != null) { enemyAI.StopCoroutine(enemyAI.circle); }        
            //    enemyAI.circle =  enemyAI.StartCoroutine(enemyAI.Circle());
            //}       
        }

        if (!enemyAI.InAttackRange())
        {
            enemyAI.SwitchState(new EnemyChase());
        }

    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {
        base.PhysicsUpdate(enemyAI);
    }
}