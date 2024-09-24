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
    public Transform noTargetDestination;

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
        Vector3 direction = GetDirection();  // Get the right direction

        TrailRenderer trail = Instantiate(projTrail, spawnPoint.position, Quaternion.identity);


        if (sm.tl.currentTarget != null)
        {
            IDamageable enemy = sm.tl.currentTarget.GetComponent<IDamageable>();
            StartCoroutine(SpawnTrail(trail, direction, enemy));
            Destroy(trail, 0.11f);
        }
        else
        {
            // If there are no targets, just fire in the player's forward direction
            StartCoroutine(SpawnTrailNoEnemy(trail, direction));
        }
    }

    public void ConcentratedNail()
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

        ConcentratedNail p = Instantiate(concentratedNail, spawnPoint.position, Quaternion.identity).GetComponent<ConcentratedNail>();
        p.transform.rotation = Quaternion.LookRotation(direction.normalized);
        p.enemy = e;
        p.destination = d;
        p.spawnPoint = spawnPoint;
        p.existingTarget = existTarget;
        p.sm = sm;
    }

    public Vector3 GetDirection()
    {
        Vector3 dir = Vector3.zero;

        if (sm.tl.currentTarget != null && !sm.tl.targetable.environment)
        {
            dir = sm.tl.currentTarget.position;
        }

        //else if (sm.pc.ClosestEnemy().transform != null)
        //{
        //    //throw new System.Exception("No CurrentTarget for Nailgun Lock.");
        //    Debug.Log("2");
        //    dir = sm.pc.ClosestEnemy().transform.position; 
        //}

        else
        {
            dir = sm.playerObj.forward;
        }

        if (bulletSpread)
        {
            dir += new Vector3(Random.Range(-spreadVariance, spreadVariance),
                Random.Range(0, spreadVariance),
                Random.Range(-spreadVariance, spreadVariance));

            //dir.Normalize();

        }

        return dir;
    }

    IEnumerator SpawnTrail(TrailRenderer trail, Vector3 enemy, IDamageable health)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;
        while (time < 1)
        {
            if (trail == null) { break; }
            trail.transform.position = Vector3.Lerp(startPos, enemy, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = enemy;
        health.TakeDamage(stats.damage, true);
        if (sm.tl.currentTarget.gameObject)
        {
            HitstopManager.Instance.TriggerHitstop(stats.hitstopAmnt, sm.tl.currentTarget.gameObject);
        }
        CinemachineShake.Instance.ShakeCamera(stats.shakeAmnt, stats.shakeDur);
        CinemachineShake.Instance.ChangeFov(stats.zoomAmnt, stats.shakeDur);
        RumbleManager.instance.RumblePulse(.05f, .15f, .1f);
        Destroy(trail.gameObject, trail.time);
    }

    IEnumerator SpawnTrailNoEnemy(TrailRenderer trail, Vector3 direction)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;
        trail.GetComponent<SphereCollider>().enabled = true;

        // Move in the forward direction continuously, not to a fixed point
        while (time < 1)
        {
            if (trail == null || !trail.GetComponent<SphereCollider>().enabled) { break; }
            trail.transform.position += direction * speed * Time.deltaTime;  // Move forward based on direction and speed
            time += Time.deltaTime;
            yield return null;
        }

        CinemachineShake.Instance.ShakeCamera(stats.shakeAmnt, stats.shakeDur);
        CinemachineShake.Instance.ChangeFov(stats.zoomAmnt, stats.shakeDur);
        RumbleManager.instance.RumblePulse(.05f, .15f, .1f);
        Destroy(trail.gameObject, trail.time);
    }
}
