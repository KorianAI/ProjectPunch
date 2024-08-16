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
                        Debug.Log("dude");
                        sm.SwitchState(new PlayerPushSlam());
                    }
                }

                else  // parry
                {
                    sm.SwitchState(new ParryState());
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
                Vector3 directionToEnemy = t.targetable.targetPoint.position - transform.position;
                float dSqrToTarget = directionToEnemy.sqrMagnitude;


                if (dSqrToTarget < distance)  // chokeslam aoe
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

}