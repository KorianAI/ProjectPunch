using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenSettings : MonoBehaviour
{
    public GameObject UI;
    public bool uiActive = false;

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
