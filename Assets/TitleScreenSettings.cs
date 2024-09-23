using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class TitleScreenSettings : MonoBehaviour
{
    [Header("Intro")]
    public VideoPlayer vp;
    public GameObject introVidObj;
    public float timeBefore;
    public GameObject blackScreen;

    [Header("Toggle Settings")]
    public GameObject settingsUI;
    public GameObject mainUI;
    public bool paused;
    public PauseTabGroup tabGroup;

    private void Awake()
    {
        //play music here

        if (introVidObj != null)
        {
            blackScreen.SetActive(true);
            vp.enabled = false;
            StartCoroutine(StartVideo());
        }

        if (settingsUI != null)
        {
            settingsUI.SetActive(false);
        }

        if (mainUI != null)
        {
            mainUI.SetActive(true);
        }
    }

    private void Start()
    {
        InputMapManager.inputActions.Player.Pause.started += ctx =>
        {
            PauseToggle();
        };

        InputMapManager.inputActions.Menus.Pause.started += ctx =>
        {
            PauseToggle();
        };
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

    public void PauseToggle()
    {
        //look for inputs for the pause (esc., controller start button)
        if (!paused) //ensures that player is not in a tutorial
        {
            PauseGame();
        }
        else if (paused)
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        paused = true;

        settingsUI.SetActive(true);
        mainUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Menus);
    }

    public void ResumeGame()
    {
        paused = false;

        settingsUI.SetActive(false);
        mainUI.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tabGroup.defaultTab.Select(); //set tab group to page 1

        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Player);
    }

    IEnumerator StartVideo()
    {
        yield return new WaitForSeconds(timeBefore);
        introVidObj.SetActive(true);
        vp.enabled = true;
        yield return new WaitForSeconds(1f);
        blackScreen.SetActive(false);
    }
}
