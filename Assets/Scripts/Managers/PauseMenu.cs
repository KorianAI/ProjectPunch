using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public bool paused;
    public PlayerMovement pm;
    public PauseTabGroup tabGroup;

    private void Start()
    {
        paused = false;
        
        InputMapManager.inputActions.Player.Pause.started += ctx =>
        {
            PauseToggle();
        };

        InputMapManager.inputActions.Menus.Pause.started += ctx =>
        {
            PauseToggle();
        };
    }

    public void PauseToggle()
    {
        //look for inputs for the pause (esc., controller start button)
        if (!paused)
        {
            paused = true;
            PauseGame();
        }
        else
        {
            paused = false;
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        //deactivate sub menus
        
        pauseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //pm.canMove = false; //movement and looking off
        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Menus);
    }

    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tabGroup.defaultTab.Select(); //set tab group to page 1
        
        //pm.canMove = true; //movement and looking on
        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Player);
    }
}
