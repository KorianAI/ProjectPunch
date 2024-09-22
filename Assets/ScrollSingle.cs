using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScrollSingle : MonoBehaviour
{
    [Header("Scroll Unlock")]
    UnlocksManager manager;
    public int scrollNo;

    [Header("Scrap")]
    private GameObject player;
    public ParticleSystem scrapParticle;

    [Header("Movement")]
    public float moveAmp = .5f;
    public float moveFreq = 1f;
    private Vector3 initPos;
    public float rotAmp = .2f;
    public float rotFreq = .5f;
    public float rotAmt = 1f;

    [Header("Scroll SFX")]
    public AK.Wwise.Event playSFX_ScrollCollect;

    void Start()
    {
        manager = GameObject.Find("UnlocksManager").GetComponent<UnlocksManager>();
        player = GameObject.Find("Player");
        initPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SFXPlay_ScrollCollect();
            manager.UnlockScroll(scrollNo);
            //vfx, sfx

            player.GetComponent<PlayerResources>().UpdateScrap(10);
            Instantiate(scrapParticle);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * moveFreq) * moveAmp + initPos.y, initPos.z);
        transform.rotation = new Quaternion(0, Mathf.Sin(Time.time * rotFreq) * rotAmp, 0, rotAmt);
    }

    public void SFXPlay_ScrollCollect()
    {
        playSFX_ScrollCollect.Post(gameObject);
    }
}
