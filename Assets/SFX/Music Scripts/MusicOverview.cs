using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOverview : MonoBehaviour
{


    PlayerResources pr;
    CombatManager ca;
    MusicManager bt;

    public static MusicOverview instance;


    [Header("Explore")]
    public AK.Wwise.Event playMusic_Explore;
    [Header("Combat 1")]
    public AK.Wwise.Event playMusic_Combat1;
    [Header("Scrap Shift")]
    public AK.Wwise.Event playMusic_ScrapShift;
    [Header("Boss1")]
    public AK.Wwise.Event playMusic_Boss1;
    [Header("Boss2")]
    public AK.Wwise.Event playMusic_Boss2;
    public AK.Wwise.Event playSFX_RS_on;
    public AK.Wwise.Event playSFX_RS_off;


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

    void Start()
    {
        pr = GetComponent<PlayerResources>();
        MusicStart();

        ca = GetComponent<CombatManager>();
        bt = GetComponent<MusicManager>();
    }

    public void Music_Explore()
    {
        playMusic_Explore.Post(gameObject);
    }

    public void Music_Combat1()
    {
        playMusic_Combat1.Post(gameObject);
    }

    public void Music_ScrapShift()
    {
        playMusic_ScrapShift.Post(gameObject);
    }

    public void Music_Boss1()
    {
        playMusic_Boss1.Post(gameObject);
    }

    public void Music_Boss2()
    {
        playMusic_Boss2.Post(gameObject);
    }

    public void MusicStart()
    {

        Music_Explore();
    }
    public void RSON()
    {
        playSFX_RS_on.Post(gameObject);
    }
    public void RSOFF()
    {
        playSFX_RS_off.Post(gameObject);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
