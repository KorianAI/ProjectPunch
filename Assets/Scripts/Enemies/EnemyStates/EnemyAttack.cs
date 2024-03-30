using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    public bool strafing;

    public override void EnterState(EnemyAI enemyAI)
    {

    }

    public override void ExitState(EnemyAI enemyAI)
    {

    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
      
        if (enemyAI.permissionToAttack)
        {
            enemyAI.agent.SetDestination(enemyAI.transform.position);
            enemyAI.transform.LookAt(new Vector3(enemyAI.playerPos.transform.position.x, enemyAI.transform.position.y, enemyAI.playerPos.transform.position.z));

            if (enemyAI.patrol != null)
            {
                
                enemyAI.StopCoroutine(enemyAI.patrol);
            }        
            enemyAI.enemy.Attack(enemyAI.playerPos.transform);
        }

        else
        {
            if (!enemyAI.patrolling)
            {
               enemyAI.patrol =  enemyAI.StartCoroutine(enemyAI.Patrol());
            }       
        }
        
    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {

    }
}