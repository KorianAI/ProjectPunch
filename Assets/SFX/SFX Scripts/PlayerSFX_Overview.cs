using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AK.Wwise.Editor;



public class PlayerSFX_Overview : MonoBehaviour
{
    PlayerResources pr;
    [Header("Movement")]
    public AK.Wwise.Event playSFX_footsteps_ground;
    public AK.Wwise.Event playSFX_footsteps_metal;
    public AK.Wwise.Event playSFX_dash;
    public AK.Wwise.Event playSFX_RS_dash;
    public AK.Wwise.Event playSFX_jump;
    public AK.Wwise.Event playSFX_land;
    public AK.Wwise.Event playSFX_launch;
    public AK.Wwise.Event playSFX_push;
    public AK.Wwise.Event playSFX_pull;

    [Header("BFG")]
    public AK.Wwise.Event playSFX_BFG_impact;
    public AK.Wwise.Event playSFX_BFG_slam;
    public AK.Wwise.Event playSFX_BFG_swing;
    public AK.Wwise.Event playSFX_BFG_lunge;
    public AK.Wwise.Event playSFX_BFGuppercut;

    [Header("Nail Gun")]
    public AK.Wwise.Event playSFX_NailG_burst;
    public AK.Wwise.Event playSFX_NailG_charged;
    public AK.Wwise.Event playSFX_NailG_chargedpull;

    [Header("Real Steel")]
    public AK.Wwise.Event playSFX_RS_impact;
    public AK.Wwise.Event playSFX_RS_slam;
    public AK.Wwise.Event playSFX_RS_swing;
    public AK.Wwise.Event playSFX_RS_slam_overdrive;
    public AK.Wwise.Event playSFX_RS_overdriveactive;
    public AK.Wwise.Event playSFX_RS_overdriveoutro;
    public AK.Wwise.Event playSFX_RS_ranged;
    public AK.Wwise.Event playSFX_RS_nail;

    bool overdriveActive = false;
    bool overdriveOutroPlayed = true;

    public AK.Wwise.Event playSFX_ScrapCollect;
    public AK.Wwise.Event playSFX_ScrollCollect;

    public AK.Wwise.Event playSFX_Kaila_LockOn;
    public AK.Wwise.Event playSFX_Kaila_LockOff;

    public AK.Wwise.Event playSFX_Rail;
    public AK.Wwise.Bank Soundbank_SFX;

    private void Start()
    {
        pr = GetComponent<PlayerResources>();
        Soundbank_SFX.Load();
    }

    //private void Update()
    //{
    //    if (pr.shift.overdrive)
    //    {
    //        overdriveActive = true;
    //        overdriveOutroPlayed = false;
    //        SFX_RS_Overdrive();
    //    }

    //    else
    //    {
    //        overdriveActive = false;
    //        SFX_RS_Overdrive();
    //    }
    //}

    public void SFX_Swing()
    {
        if (pr.scrapShift)
        {
            playSFX_BFG_swing.Post(gameObject);
        }

        else
        {
            playSFX_BFG_swing.Post(gameObject);
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
        }
    }

    public void SFX_BFG_Uppercut()
    {
        playSFX_BFGuppercut.Post(gameObject);
    }

    public void SFX_RS_Overdrive()
    {
        if (pr.shift.overdrive)
        {
            playSFX_RS_overdriveactive.Post(gameObject);
        }

        else
        {
            if (overdriveActive == false && overdriveOutroPlayed == false)
            {
                playSFX_RS_overdriveoutro.Post(gameObject);
                overdriveOutroPlayed = true;
            }

        }

    }

    //public void SFX_RS_States()
    //{
    //    if (pr.scrapShift)
    //    {
    //        playSFX_RS_off.Post(gameObject);
    //    }

    //    else
    //    {
    //        playSFX_RS_on.Post(gameObject);
 
    //    }
    //}

    public void SFX_NailG_Burst()
    {
        playSFX_NailG_burst.Post(gameObject);
    }

    public void SFX_NailG_Charged()
    {
        playSFX_NailG_charged.Post(gameObject);
    }

    public void SFX_NailG_ChargedPull()
    {
        playSFX_NailG_chargedpull.Post(gameObject);
    }

    public void SFX_Footsteps()
    {
        playSFX_footsteps_ground.Post(gameObject);
    }

    public void SFX_Jump()
    {
        playSFX_jump.Post(gameObject);
    }

    public void SFX_Land()
    {
        playSFX_land.Post(gameObject);
    }

    public void SFX_Dash()
    {
        if (pr.scrapShift)
        {
            playSFX_RS_dash.Post(gameObject);
        }

        else
        {
            playSFX_dash.Post(gameObject);

        }
    }

    public void SFX_BouncePad_Launch()
    {
        playSFX_launch.Post(gameObject);
    }

    public void SFX_Push()
    {
        playSFX_push.Post(gameObject);
    }

    public void SFX_Pull()
    {
        playSFX_pull.Post(gameObject);
    }

    public void SFX_ScrapCollect()
    {
        playSFX_ScrapCollect.Post(gameObject);
    }

    public void SFX_ScrollCollect()
    {
        playSFX_ScrollCollect.Post(gameObject);
    }

    public void SFX_Kaila_LockOn()
    {
        playSFX_Kaila_LockOn.Post(gameObject);
    }

    public void SFX_Kaila_LockOff()
    {
        playSFX_Kaila_LockOff.Post(gameObject);
    }

    public void SFX_Rail()
    {
        playSFX_Rail.Post(gameObject);
    }

    public void SFX_RS_Ranged()
    {
        playSFX_RS_ranged.Post(gameObject);
    }
    public void SFX_RS_Nail()
    {
        playSFX_RS_nail.Post(gameObject);
    }




}
