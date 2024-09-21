using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RSProjectile : MonoBehaviour
{
    public Transform destination;
    public float speed;
    public Transform spawnPoint;

    public LayerMask enemyLayer;

    public AttackStats explosionStats;
    public GameObject explosionPrefab;
    public bool existingTarget;

    private void Start()
    {
        float distance = Vector3.Distance(spawnPoint.position, destination.position);
        float dur = distance / speed;
        transform.DOMove(destination.position, dur).OnComplete(Detonate);
        Destroy(gameObject, dur + .2f);
    }

    public void Detonate()
    {
        transform.DOKill();
        GameObject vfx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Collider[] enemies = Physics.OverlapSphere(transform.position, 4f, enemyLayer);
        foreach (Collider c in enemies)
        {
            c.GetComponent<IDamageable>().TakeDamage(explosionStats.damage);

            HitstopManager.Instance.TriggerHitstop(explosionStats.hitstopAmnt, gameObject, c.gameObject);
            CinemachineShake.Instance.ShakeCamera(explosionStats.shakeAmnt, explosionStats.shakeAmnt);
            CinemachineShake.Instance.ChangeFov(explosionStats.zoomAmnt, explosionStats.shakeDur);
            RumbleManager.instance.RumblePulse(.15f, .25f, .3f);

            if (c.GetComponent<EnemyHealth>() != null)
            {
                c.GetComponent<EnemyHealth>().GetStunned(.2f);

            }
        }

        Destroy(vfx, 1f); 
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!existingTarget)
            {
                Detonate();
            }
        }
    }
}
