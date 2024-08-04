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
                    if (command.Direction.y < -.5f)
                    {

                    }
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
