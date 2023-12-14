using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagnetTesting : MonoBehaviour
{
    public float raycastRange;
    public Transform pullPoint;
    RaycastHit hit;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PullEnemy();
        }
        
    }

    void PullEnemy()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastRange))
        {
            GameObject enemy = hit.collider.gameObject;
            if (enemy.CompareTag("Enemy"))
            {
                enemy.transform.DOMove(pullPoint.position, .5f);
            }
        }
    }
}
