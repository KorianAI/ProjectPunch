using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenSettings : MonoBehaviour
{
    [Header("Settings UI")]
    public GameObject UI;
    public bool uiActive = false;

    [Header("Intro")]
    public GameObject introVidObj;

    private void Awake()
    {
        //play music here

        if (introVidObj != null)
        {
            introVidObj.SetActive(true);
        }
    }

    public void ToggleUI()
    {
        if (uiActive)
        {
            uiActive = false;
            UI.SetActive(false);
        }
        else
        {
            uiActive = true;
            UI.SetActive(true);
        }
    }
}
