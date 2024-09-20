using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOverview : MonoBehaviour
{


    PlayerResources pr;
    [Header("Explore")]
    public AK.Wwise.Event playMusic_Explore;
    [Header("Combat 1")]
    public AK.Wwise.Event playMusic_Combat1;
    [Header("Scrap Shift")]
    public AK.Wwise.Event playMusic_ScrapShift;

    // Start is called before the first frame update
    void Start()
    {
        pr = GetComponent<PlayerResources>();
        MusicStates();
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

    public void MusicStates()
    {
        if (pr.scrapShift)
        {
            Music_ScrapShift();
        }

        else
        {
            Music_Explore();
        }

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
