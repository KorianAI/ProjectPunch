using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    MusicManager manager;
    [SerializeField] AudioClip newMusic;

    private void Start()
    {
        manager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.StartCoroutine(manager.SwapTrack(newMusic));
        }
    }
}
