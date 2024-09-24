using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosiveBarrel : MonoBehaviour, IDamageable, IMagnetisable
{
    public GameObject explosionVFX;
    public LayerMask targetableLayer;
    public float damage;
    bool exploded = false;
    

    private PlayerStateManager ps;
    //public AK.Wwise.Event playSFX_BarrelExplosion;
    public string eventName = "PlaySFX_BarrelExplosion";

    private void Start()
    {
        ps = GameObject.Find("Player").GetComponent<PlayerStateManager>();
        
    }

    public void TakeDamage(float damage, bool ranged)
    {
        if (exploded) return;
        exploded = true;
        Detonate();
    }

    void Detonate()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.identity);

        Debug.Log("how man ytimes ");

        Collider[] player = Physics.OverlapSphere(transform.position, 2, targetableLayer);
        foreach (Collider c in player)
        {
            c.GetComponent<IDamageable>().TakeDamage(damage, false);
            //sfx - explode
            
            RumbleManager.instance.RumblePulse(.25f, 1f, .25f);
        }

        ps.tl.CancelLock();
        GetComponent<Targetable>().ResetColor();

        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject);
        SFXplosion();
    }

    public void Pull(PlayerStateManager player)
    {
        transform.DOMove(player.pullPosition.position, 1f).OnComplete(() => { ps.pulling = false; });
        transform.DOShakeRotation(1, 15f, 10, 90);
    }

    public void Push(PlayerStateManager player)
    {
        transform.DOMove(transform.position + player.orientation.forward, 1f);
        transform.DOShakeRotation(1, 15f, 10, 90);
    }

    public void SFXplosion()
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }
}
