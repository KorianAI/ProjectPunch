using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircle : EnemyState
{
    bool canCircle;
    bool rePositioning;
    Vector3 randomDestination;



    public override void EnterState(EnemyAI enemyAI)
    {
        base.EnterState(enemyAI);
        ai.agent.angularSpeed = 0;
        StartCircling();
    }

    public override void ExitState(EnemyAI enemyAI)
    {     
        base.ExitState(enemyAI);

    }

    public override void FrameUpdate(EnemyAI enemyAI)
    {
        base.FrameUpdate(enemyAI);


        if (!ai.InAttackRange())
        {
            ai.agent.SetDestination(ai.transform.position);
            ai.SwitchState(new EnemyChase());
        }

        if (rePositioning)
        {
            Circle();
            if (ai.agent.remainingDistance <= ai.agent.stoppingDistance)
            {
                ai.agent.angularSpeed = 270;
                ai.SwitchState(new EnemyAttackWait());
            }
        }

        else
        {
            ai.agent.angularSpeed = 270;
            ai.transform.LookAt(new Vector3(ai.playerPos.transform.position.x, ai.transform.position.y, ai.playerPos.transform.position.z));
        }


    }

    public override void PhysicsUpdate(EnemyAI enemyAI)
    {
        base.PhysicsUpdate(enemyAI);
    }

    void StartCircling()
    {

        rePositioning = true;
        randomDestination = ai.GetRandomPointAroundPlayer(ai.playerPos.transform.position, ai.circleRadius);
    }

    public void Circle()
    {
        ai.agent.SetDestination(randomDestination);

        ai.transform.LookAt(new Vector3(ai.playerPos.transform.position.x, ai.transform.position.y, ai.playerPos.transform.position.z));
    }
}
