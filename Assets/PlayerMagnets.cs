using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnets : MonoBehaviour
{
    PlayerStateManager sm;
    PlayerCombat pc;
    PlayerMovement pm;
    PlayerInputHandler ih;
    TargetCams t;

    public float distance;

    private void Start()
    {
        sm = GetComponent<PlayerStateManager>();
        pc = GetComponent<PlayerCombat>();
        pm = GetComponent<PlayerMovement>();
        ih = GetComponent<PlayerInputHandler>();
        t = GetComponent<TargetCams>();
    }

    public void PushInput(InputCommand command, bool grounded)
    {
        //Debug.Log("fuck");

        if (t.targetable != null)
        {
            if (!t.targetable.environment)
            {
                if (command.Direction.y < -0.5f)
                {
                    if (grounded) // knockup enemy
                    {
                        sm.SwitchState(new PushKnockup());
                    }

                    else  // slam down
                    {
                        sm.SwitchState(new PushKnockup());
                    }
                }

                else  // parry
                {
                    sm.SwitchState(new PushKnockup());
                }
            }

            else
            {
                sm.SwitchState(new PushState());
                //Debug.Log("breh");
            }
        }
    }

    public void PullInput(InputCommand command)
    {
        if (t.targetable != null)
        {
            if (!t.targetable.environment)
            {
                var result = pc.ClosestEnemy();
                Vector3 closestEnemyPosition = result.position;
                float closestEnemyDistance = result.distance;

                if (closestEnemyDistance < distance)  // chokeslam aoe
                {
                    sm.SwitchState(new PullEnemyState());
                }

                else
                {
                    sm.SwitchState(new PullEnemyState());  // pull enemy
                }
            }

            else
            {
                sm.SwitchState(new PullState());  // pull to environment
            }
        }
    }

    public Vector3 ClosestMangeticObject()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 5f);
        Collider closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            float dSqrToTarget = directionToEnemy.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            float closestDistance = Mathf.Sqrt(closestDistanceSqr); // Calculate the actual distance
            return (closestEnemy.transform.position);
        }

        return (Vector3.zero);
    }
}
