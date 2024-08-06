using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConcentratedNail : MonoBehaviour, IMagnetisable, IParriable
{
    public EnemyHealth enemy;
    public Transform destination;
    public float speed;
    public bool stuck;
    public float dur;

    public Transform spawnPoint;
    public PlayerStateManager sm;

    public GameObject explosionPrefab;
    public float explosionRange;
    public float explosionDamage;
    public LayerMask enemyLayer;
    public AttackStats explosionStats;

    private void Start()
    {
        float distance = Vector3.Distance(spawnPoint.position, destination.position);
        float dur = distance / speed;
        transform.DOMove(destination.position, dur).OnComplete(PierceTarget);
    }

    private void PierceTarget()
    {
        if (enemy.nail != null)
        {
            enemy.nail.DestroyNail();
        }
        transform.SetParent(destination);
        enemy.nailImpaled = true;
        enemy.nail = this;
    }

    public void Pull(PlayerStateManager player)
    {
        
    }

    public void Push(PlayerStateManager player)
    {
        
    }

    public void DestroyNail()
    {
        Destroy(gameObject);

    }

    public void Detonate()
    {
        GameObject vfx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Collider[] enemies = Physics.OverlapSphere(transform.position, 4f, enemyLayer);
        foreach (Collider c in enemies)
        {
            c.GetComponent<IDamageable>().TakeDamage(explosionDamage);

            HitstopManager.Instance.TriggerHitstop(explosionStats.hitstopAmnt, gameObject, c.gameObject);
            CinemachineShake.Instance.ShakeCamera(explosionStats.shakeAmnt, explosionStats.shakeAmnt);
            CinemachineShake.Instance.ChangeFov(explosionStats.zoomAmnt, explosionStats.shakeDur);
            RumbleManager.instance.RumblePulse(.15f, .25f, .3f);

            if (c.GetComponent<EnemyHealth>() != null)
            {
                c.GetComponent<EnemyHealth>().GetStunned(.2f);

            }
        }

        Destroy(gameObject);
    }

    public void ReturnToSpawn()
    {
        Vector3 direction = destination.position - sm.transform.position;
        transform.rotation = Quaternion.LookRotation(direction.normalized);
        transform.SetParent(null);
        float distance = Vector3.Distance(transform.position, spawnPoint.position);
        float dur = distance / speed;
        transform.DOMove(spawnPoint.position, dur);
    }

    public void Parry()
    {
        Debug.Log("uwaahhhh parried!");
        float distance = Vector3.Distance(spawnPoint.position, destination.position);
        float dur = distance / speed;
        transform.DOMove(destination.position, dur).OnComplete(Detonate);
    }
}
