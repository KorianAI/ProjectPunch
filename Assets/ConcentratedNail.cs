using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConcentratedNail : MonoBehaviour, IMagnetisable
{
    public EnemyHealth enemy;
    public Transform destination;
    public float speed;
    public bool stuck;
    public float dur;

    public Transform spawnPoint;
    public PlayerStateManager sm;



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

    public void ReturnToSpawn()
    {
        Vector3 direction = destination.position - sm.transform.position;
        transform.rotation = Quaternion.LookRotation(direction.normalized);
        transform.SetParent(null);
        float distance = Vector3.Distance(transform.position, spawnPoint.position);
        float dur = distance / speed;
        transform.DOMove(spawnPoint.position, dur);
    }
}
