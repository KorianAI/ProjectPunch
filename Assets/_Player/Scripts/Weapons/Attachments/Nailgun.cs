using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nailgun : Attachment
{
    public bool bulletSpread;
    public float spreadVariance;
    public Transform spawnPoint;
    public TrailRenderer projTrail;
    private ParticleSystem shootSystem;

    public LayerMask hittable;

    public GameObject concentratedNail;
    public float cnDur;
    public float speed;

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        
    }

    public override void WeaponInput(InputCommand command, bool grounded, int index)
    {
        if (command.Type == InputType.X)
        {
            sm.SwitchState(new NLG_G1());
        }

        else if (command.Type == InputType.xH)
        {
            sm.SwitchState(new NLG_G2());
        }


    }

    public void Shoot()
    {
        Vector3 direction = GetDirection();

        TrailRenderer trail = Instantiate(projTrail, spawnPoint.position, Quaternion.identity);
        IDamageable enemy = sm.tl.currentTarget.GetComponent<IDamageable>();
        StartCoroutine(SpawnTrail(trail, direction, enemy));

    }

    public void ConcentratedNail()
    {
        if (sm.tl.currentTarget != null && !sm.tl.targetable.environment)
        {
            EnemyHealth enemy = sm.tl.currentTarget.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                Vector3 direction = sm.tl.currentTarget.position - transform.position;
                ConcentratedNail p = Instantiate(concentratedNail, spawnPoint.position, Quaternion.identity).GetComponent<ConcentratedNail>();
                p.transform.rotation = Quaternion.LookRotation(direction.normalized);
                p.enemy = enemy;
                p.destination = sm.tl.targetPoint;
                p.spawnPoint = spawnPoint;
                p.sm = sm;
            }
        }
 
    }

    public Vector3 GetDirection()
    {
        if (sm.tl.currentTarget != null && !sm.tl.targetable.environment)
        {
            Vector3 dir = sm.tl.currentTarget.position;

            if (bulletSpread)
            {
                dir += new Vector3(Random.Range(-spreadVariance, spreadVariance),
                    Random.Range(0, spreadVariance),
                    Random.Range(-spreadVariance, spreadVariance));

                //dir.Normalize();

            }

            return dir;
        }

        else
        {
            throw new System.Exception("No CurrentTarget for Nailgun Lock.");
        }
        
    }

    IEnumerator SpawnTrail(TrailRenderer trail, Vector3 enemy, IDamageable health)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, enemy, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = enemy;
        health.TakeDamage(stats.damage);
        HitstopManager.Instance.TriggerHitstop(stats.hitstopAmnt, sm.tl.currentTarget.gameObject);
        CinemachineShake.Instance.ShakeCamera(stats.shakeAmnt, stats.shakeDur);
        CinemachineShake.Instance.ChangeFov(stats.zoomAmnt, stats.shakeDur);
        RumbleManager.instance.RumblePulse(.05f, .15f, .1f);
        Destroy(trail.gameObject, trail.time);
    }
}
