using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretProjectile : MonoBehaviour, IParriable
{
    public GameObject explosionVFX;
    public LayerMask playerLayer;
    public float damage;
    public float destroyTime = 1f;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        RumbleManager.instance.RumblePulse(.10f, .5f, .10f);
        //sfx - fire

        if (collision.gameObject.layer == whatisGround)
        {
            Instantiate(rumbleVFX, transform.position, Quaternion.identity);
        }

        Detonate();
    }

    public void Parry()
    {
        float distance = Vector3.Distance(playerPos.transform.position, spawnPoint);
        float dur = distance / speed;
        transform.DOMove(spawnPoint, dur).OnComplete(Detonate);
    }


    void Detonate()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.identity);

        Collider[] player = Physics.OverlapSphere(transform.position, 2, playerLayer);
        foreach (Collider c in player)
        {
            c.GetComponent<IDamageable>().TakeDamage(damage);
            //sfx - explode
            RumbleManager.instance.RumblePulse(.25f, 1f, .25f);
        }

        Destroy(gameObject);
    }
}
