using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealSteel : WeaponInfo
{
    // Overdrive
    bool overdrive;
    float maxOverdrive;
    float currentOverdrive;

    // shockblast
    public GameObject projectile;
    public GameObject muzzleVFX;
    public Transform projSpawn;

    public override void WeaponInput(InputCommand command, bool grounded, int index)
    {

        if (grounded)
        {
            if (command.Type == InputType.Y)
            {
                BaseCombo(index);
            }

            else if (command.Type == InputType.X)
            {
                sm.SwitchState(new RS_X());
            }

            else if (command.Type == InputType.xH)
            {
                // RS_XH; // change to better nail - Nailgun
            }
        }

        else
        {
            if (command.Type == InputType.Y)
            {
                AirCombo(index);
            }

        }
    }

    public void BaseCombo(int index)
    {
        stats = baseComboStats[index];

        if (index == 0)
        {
            sm.SwitchState(new RS_G1());
        }

        else if (index == 1)
        {
            if (sm.pc.pauseAttack)
            {
                sm.SwitchState(new RS_G4());
            }

            else
            {
                sm.SwitchState(new RS_G2());
            }

        }

        else if (index == 2)
        {
            if (sm.pc.pauseAttack)
            {
                sm.SwitchState(new RS_G6());
            }

            else
            {
                sm.SwitchState(new RS_G3());
            }

        }

        else if (index == 3)
        {
            sm.SwitchState(new RS_G5());
        }
    }

    public void AirCombo(int index)
    {
        stats = airComboStats[index];

        if (index == 0)
        {
            sm.SwitchState(new BFG_A1());
        }

        else if (index == 1)
        {
            sm.SwitchState(new BFG_A2());
        }

        else if (index == 2)
        {
            sm.SwitchState(new BFG_A3());
        }
    }

    public void PunchBlast()
    {
        if (sm.tl.currentTarget != null && !sm.tl.targetable.environment)
        {
            EnemyHealth enemy = sm.tl.currentTarget.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                Vector3 direction = sm.tl.currentTarget.position - transform.position;
                RSProjectile p = Instantiate(projectile, projSpawn.position, Quaternion.identity).GetComponent<RSProjectile>();
                GameObject muzzle = Instantiate(muzzleVFX, projSpawn.position, Quaternion.identity); 
                p.transform.rotation = Quaternion.LookRotation(direction.normalized);
                p.destination = sm.tl.targetPoint;
                p.spawnPoint = projSpawn;

                Destroy(muzzle, .2f);
            }
        }

    }
}
