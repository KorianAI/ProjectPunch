using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerCombat : MonoBehaviour
{
    PlayerStateManager _sm;
    PlayerResources resources;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;

    public GameObject hitVFX;

    public float launchHeight;
    public float launchDuration;

    private void Start()
    {
        _sm = GetComponent<PlayerStateManager>();
        resources = GetComponent<PlayerResources>();
    }
    public void CheckForEnemies()
    {
        AttackStats modeStats = resources.mode.stats;
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider c in enemies)
        {

            c.GetComponent<IDamageable>().TakeDamage(modeStats.damage);

            _sm.attackHit = true;
            HitstopManager.Instance.TriggerHitstop(modeStats.hitstopAmnt, gameObject, c.gameObject);
            CinemachineShake.Instance.ShakeCamera(modeStats.shakeAmnt, modeStats.shakeAmnt);
            CinemachineShake.Instance.ChangeFov(modeStats.zoomAmnt, modeStats.shakeDur);
            RumbleManager.instance.RumblePulse(.15f, .25f, .3f);
            transform.DOKill();         

            GameObject hitParticle = Instantiate(hitVFX, c.transform);

            if (c.GetComponent<EnemyHealth>() != null)
            {
                c.GetComponent<EnemyHealth>().GetStunned(.2f);

            }

        }
    }

    public void Knockup(float type)
    {
        AttackStats modeStats = resources.mode.stats;
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider c in enemies)
        {
            HitstopManager.Instance.TriggerHitstop(modeStats.hitstopAmnt, gameObject, c.gameObject);
            CinemachineShake.Instance.ShakeCamera(modeStats.shakeAmnt, modeStats.shakeAmnt);
            RumbleManager.instance.RumblePulse(.15f, .25f, .3f);
            transform.DOKill();
            Vector3 launchPosition = new Vector3(c.transform.position.x, c.transform.position.y + launchHeight, c.transform.position.z);
            c.transform.DOMove(launchPosition, launchDuration).SetEase(Ease.OutQuad);

            if (c.GetComponent<EnemyHealth>() != null)
            {
                c.GetComponent<EnemyHealth>().GetStunned(.2f);

            }

        }

        if (type == 2)
        {
            Vector3 launchPosition = new Vector3(transform.position.x, transform.position.y + launchHeight, transform.position.z);
            transform.DOMove(launchPosition, launchDuration).SetEase(Ease.OutQuad);
        }
    }
}
