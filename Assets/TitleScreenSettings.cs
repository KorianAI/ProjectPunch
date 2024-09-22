using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleScreenSettings : MonoBehaviour
{
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

    public void ToggleUI(GameObject ui)
    {
        if (ui.activeInHierarchy)
        {
            ui.SetActive(false);
        }
        else
        {
            ui.SetActive(true);
        }
    }

    public void SelectButton(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);
    }
}
