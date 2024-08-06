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
    public PlayerStateManager sm;
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
        if (sm != null && !sm.tutIsActive && !paused) //ensures that player is not in a tutorial
        {
            PauseGame();
        }
        else if (sm != null && !sm.tutIsActive && paused)
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        paused = true;

        pauseUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Menus);
    }

    public void ResumeGame()
    {
        paused = false;

        pauseUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tabGroup.defaultTab.Select(); //set tab group to page 1
        
        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Player);
    }
}
