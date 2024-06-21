using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nailgun : WeaponInfo
{
    public bool bulletSpread;
    public float spreadVariance;
    public Transform spawnPoint;
    public TrailRenderer projTrail;
    private ParticleSystem shootSystem;

    public LayerMask hittable;

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Shoot();
        }
    }

    public override void WeaponInput(InputCommand command, bool grounded)
    {
        if (grounded && command.Type == InputType.X)
        {
            sm.SwitchState(new NLG_G1());
        }

        else
        {
            sm.SwitchState(sm.inAirState);
        }
    }

    public void Shoot()
    {
        //shootSystem.Play();
        Vector3 direction = GetDirection();

        TrailRenderer trail = Instantiate(projTrail, spawnPoint.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, direction));

    }

    public Vector3 GetDirection()
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

    IEnumerator SpawnTrail(TrailRenderer trail, Vector3 enemy)
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
        Destroy(trail.gameObject, trail.time);
    }
}
