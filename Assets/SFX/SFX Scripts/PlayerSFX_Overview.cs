using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise.Editor;



public class PlayerSFX_Overview : MonoBehaviour
{
    PlayerResources pr;
    [Header("BFG")]
    public AK.Wwise.Event playSFX_BFG_impact;
    public AK.Wwise.Event playSFX_BFG_slam;
    public AK.Wwise.Event playSFX_BFG_swing;
    public AK.Wwise.Event playSFX_BFG_lunge;

    [Header("Real Steel")]
    public AK.Wwise.Event playSFX_RS_impact;
    public AK.Wwise.Event playSFX_RS_slam;
    public AK.Wwise.Event playSFX_RS_swing;
    public AK.Wwise.Event playSFX_RS_slam_overdrive;
    public AK.Wwise.Event playSFX_RS_overdriveactive;
    public AK.Wwise.Event playSFX_RS_overdriveoutro;


    private void Start()
    {
        pr = GetComponent<PlayerResources>();
    }

    public void SFX_Swing()
    {
        if (pr.scrapShift)
        {
            playSFX_BFG_swing.Post(gameObject);
        }

        else
        {
            playSFX_BFG_swing.Post(gameObject);
            Debug.Log("Playing sound");
        }
    }

    public void SFX_Slam()
    {
        if (pr.scrapShift) //normal
        {
            playSFX_RS_slam.Post(gameObject);
        }

        else if (pr.shift.overdrive) //overdrive
        {
            playSFX_RS_slam_overdrive.Post(gameObject);
        }

        else
        {
            playSFX_BFG_slam.Post(gameObject);
        }
    }

    public void SFX_Impact()
    {
        if (pr.scrapShift)
        {
            playSFX_RS_impact.Post(gameObject);
        }

        else
        {
            playSFX_BFG_impact.Post(gameObject);
            Debug.Log("Playing sound");
        }
    }

    public void SFX_RS_Overdrive()
    {
        if (pr.shift.overdrive)
        {
            playSFX_RS_overdriveactive.Post(gameObject);
        }

        else
        {
            playSFX_RS_overdriveoutro.Post(gameObject);
        }

    }
}
