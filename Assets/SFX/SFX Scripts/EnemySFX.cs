using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    [Header("Muzzler")]
    public AK.Wwise.Event playSFX_MuzzlerKick;

    [Header("Turret")]
    public AK.Wwise.Event playSFX_TurretFire;
    public AK.Wwise.Event playSFX_TurretHit;
    public AK.Wwise.Event playSFX_TurretDestroyed;
    public AK.Wwise.Event playSFX_TurretLockOn;

    public static EnemySFX instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SFX_MuzzlerKick()
    {
        playSFX_MuzzlerKick.Post(gameObject);
    }
    public void SFX_TurretFire()
    {
        playSFX_TurretFire.Post(gameObject);
    }

    public void SFX_TurretHit()
    {
        playSFX_TurretHit.Post(gameObject);
    }

    public void SFX_TurretDestroyed()
    {
        playSFX_TurretDestroyed.Post(gameObject);
    }

    public void SFX_TurretLockOn()
    {
        playSFX_TurretLockOn.Post(gameObject);
    }

}
