using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSingle : MonoBehaviour
{
    UnlocksManager manager;
    public int scrollNo;

    void Start()
    {
        manager = GameObject.Find("UnlocksManager").GetComponent<UnlocksManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.UnlockScroll(scrollNo);
            //vfx, sfx
            Destroy(gameObject);
        }
    }
}
