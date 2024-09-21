using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealSteel : WeaponInfo
{
    [Header("Overdrive")]
    public bool overdrive;
    public float maxOverdrive;
    public float currentOverdrive;
    public bool overdriveDecrease;
    public float overdriveDecreaseAmnt;
    float overdriveDecreaseTimer;
    public float overdriveDecreaseCooldown;
    public GameObject overdriveUI;
    public Image overdriveImage;
    public GameObject[] overdriveVFX;

    [Header("Shockblast")]
    public GameObject projectile;
    public GameObject muzzleVFX;
    public Transform projSpawn;
    public Transform noTargetDestination;

    [Header("RSNail")]
    public Transform nailSpawn;
    public GameObject nailProj;


    public ParticleSystem[] overdriveParticles;

    public override void WeaponInput(InputCommand command, bool grounded, int index)
    {

        if (grounded)
        {
            if (command.Type == InputType.Y)
            {
                if (command.Direction.y < -0.5f)
                {
                    sm.SwitchState(new RS_Slam());
                }

                else
                {
                    BaseCombo(index);
                }

            }

            else if (command.Type == InputType.X)
            {
                sm.SwitchState(new RS_X());
            }

            else if (command.Type == InputType.xH)
            {
                sm.SwitchState(new RS_XH());
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
        Vector3 direction = Vector3.zero;
        Transform d = null;
        bool existTarget = true;

        if (sm.tl.currentTarget != null && !sm.tl.targetable.environment)
        {
            EnemyHealth enemy = sm.tl.currentTarget.GetComponent<EnemyHealth>();
            direction = sm.tl.currentTarget.position - transform.position;
            d = sm.tl.targetPoint;
            existTarget = true;
        }

        else
        {
            direction = sm.playerObj.forward;
            d = noTargetDestination;
            existTarget = false;
        }

        RSProjectile p = Instantiate(projectile, projSpawn.position, Quaternion.identity).GetComponent<RSProjectile>();
        GameObject muzzle = Instantiate(muzzleVFX, projSpawn.position, Quaternion.identity);
        p.transform.rotation = Quaternion.LookRotation(direction.normalized);
        p.spawnPoint = projSpawn;
        p.destination = d;
        p.existingTarget = existTarget;
        Destroy(muzzle, .2f);
    }



    private void Update()
    {
        if (overdriveDecrease)
        {
            if (overdriveDecreaseTimer >= 0)
            {
                overdriveDecreaseTimer -= Time.deltaTime;

                if (overdriveDecreaseTimer <= 0)
                {
                    UpdateOverdrive(overdriveDecreaseAmnt);
                    overdriveDecreaseTimer = overdriveDecreaseCooldown;
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            UpdateOverdrive(100);
        }
    }

    #region Overdrive
    private void ActivateOverdrive(bool on)
    {
        if (on)
        {
            currentOverdrive = maxOverdrive;
            overdriveDecreaseTimer = overdriveDecreaseCooldown;
            overdrive = true;
            sm.anim.speed = 1.5f;            
            //foreach (GameObject g in overdriveVFX)
            //{
            //    g.SetActive(true); ;
            //}
        }

        else if (!on)
        {
            overdrive = false;
            sm.anim.speed = 1f;
            if (overdriveUI != null)
            {
                overdriveUI.SetActive(false);
            }
            //foreach (GameObject g in overdriveVFX)
            //{
            //    g.SetActive(false); ;
            //}
        }

    }

    public void UpdateOverdrive(float amount)
    {
        if (amount > 0)
        {
            overdriveDecrease = false;
            StopAllCoroutines();
            StartCoroutine("ResetOverdriveDecrease");

        }
        currentOverdrive += amount;
        if (currentOverdrive + amount > maxOverdrive)
        {
            ActivateOverdrive(true); 
        }
        if (currentOverdrive + amount < 0)
        {
            currentOverdrive = 0;
            if (overdrive)
            {
                ActivateOverdrive(false);
            }

        }

        UpdateOverdriveUI();
    }

    public void RSNail()
    {
        Vector3 direction = Vector3.zero;
        EnemyHealth e = null;
        Transform d = null;
        bool existTarget = true;

        if (sm.tl.currentTarget != null && !sm.tl.targetable.environment)
        {
            EnemyHealth enemy = sm.tl.currentTarget.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                direction = sm.tl.currentTarget.position - transform.position;
                e = enemy;
            }

            d = sm.tl.targetPoint;
            existTarget = true;
        }

        else
        {
            direction = sm.playerObj.forward;
            d = noTargetDestination;
            existTarget = false;
        }

        ConcentratedNail p = Instantiate(nailProj, nailSpawn.position, Quaternion.identity).GetComponent<ConcentratedNail>();
        p.transform.rotation = Quaternion.LookRotation(direction.normalized);
        p.enemy = e;
        p.destination = d;
        p.spawnPoint = nailSpawn;
        p.existingTarget = existTarget;
        p.sm = sm;

    }

    void UpdateOverdriveUI()
    {
        overdriveImage.fillAmount = currentOverdrive / maxOverdrive;

        UpdateOverdriveParticles();
    }

    public void UpdateOverdriveParticles()
    {

        float minOverdriveThreshold = 0.30f;  // 30%
        float maxAlpha = 200f / 255f;         

        foreach (ParticleSystem p in overdriveParticles)
        {
            if (p == null) continue;

            var mainModule = p.main;
            ParticleSystem.MinMaxGradient startColor = mainModule.startColor;
            Color color = startColor.color;

            if (currentOverdrive / maxOverdrive >= minOverdriveThreshold)
            {
                float adjustedOverdrive = (currentOverdrive - (maxOverdrive * minOverdriveThreshold)) / (maxOverdrive * (1 - minOverdriveThreshold));
                color.a = Mathf.Clamp(adjustedOverdrive, 0f, 1f) * maxAlpha;
            }
            else
            {
                color.a = 0f;
            }

            startColor.color = color;
            mainModule.startColor = startColor;
        }
    }

    private IEnumerator ResetOverdriveDecrease()
    {
        yield return new WaitForSeconds(1.5f);
        overdriveDecrease = true;
    }
    #endregion
}
