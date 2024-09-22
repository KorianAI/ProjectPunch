using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretProjectile : MonoBehaviour, IParriable
{
    public GameObject firedVFX;
    public GameObject explosionVFX;
    public LayerMask targetableLayer;
    public float damage;
    public float parriedDamage = 50f;
    public bool parried = false;
    public float destroyTime = 1f;
    Rigidbody rb;

    [Header("Hit Ground")]
    public LayerMask whatisGround;
    public GameObject rumbleVFX;

    [Header("Parrying")]
    Vector3 spawnPoint;
    GameObject playerPos;
    public float speed = 65;

    private void Awake()
    {
        spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        playerPos = PlayerStateManager.instance.gameObject;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Instantiate(firedVFX, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        //rb.isKinematic = true;
        RumbleManager.instance.RumblePulse(.10f, .5f, .10f);
        //sfx - fire

        //if (collision.gameObject.layer == whatisGround)
        //{
        //    Instantiate(rumbleVFX, transform.position, Quaternion.identity);
        //}

        Detonate();
    }

    public void Parry()
    {
        parried = true;
        damage = parriedDamage;
        float distance = Vector3.Distance(playerPos.transform.position, spawnPoint);
        float dur = distance / speed;
        transform.DOMove(spawnPoint, dur).OnComplete(Detonate);
    }


    void Detonate()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.identity);

        Collider[] targets = Physics.OverlapSphere(transform.position, 2, targetableLayer);
        if (targets != null)
        {
            foreach (Collider c in targets)
            {
                if (c.GetComponent<PlayerResources>() || c.GetComponent<EnemyHealth>() || c.GetComponent<TurretHealth>())
                {
                    c.GetComponent<IDamageable>().TakeDamage(damage);
                    //sfx - explode
                    RumbleManager.instance.RumblePulse(.25f, 1f, .25f);
                }
            }
        }

        Destroy(gameObject);
    }
}
