using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    public bool strafing;

    bool playerLeft;

    public override void EnterState(EnemyAI enemyAI)
    {
        enemyAI.available = true;
        enemyAI.enemy.anim.SetBool("Patrolling", true);
    }

    public override void ExitState(EnemyAI enemyAI)
    {
        enemyAI.enemy.anim.SetBool("Patrolling", false);
    }


    public override void FrameUpdate(EnemyAI enemyAI)
    {

        enemyAI.transform.LookAt(new Vector3(enemyAI.playerPos.transform.position.x, enemyAI.transform.position.y, enemyAI.playerPos.transform.position.z));

        if (enemyAI.permissionToAttack)
        {
            
            enemyAI.agent.SetDestination(enemyAI.transform.position);

            if (enemyAI.patrol != null)
            {             
                enemyAI.StopCoroutine(enemyAI.patrol);
            }        
            enemyAI.enemy.Attack(enemyAI.playerPos.transform);
            enemyAI.rePositioning = false;

            //enemyAI.audioManager.BaseSwing();
        }

        else
        {
            //if (!enemyAI.rePositioning && enemyAI.manager.chosenEnemy != null)
            //{
            //    if (enemyAI.patrol != null) { enemyAI.StopCoroutine(enemyAI.patrol); }        
            //    enemyAI.patrol =  enemyAI.StartCoroutine(enemyAI.Patrol());
            //}       
        }

        if (!enemyAI.InAttackRange())
        {
            enemyAI.SwitchState(enemyAI.chaseState);
        }

    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {

    }
}