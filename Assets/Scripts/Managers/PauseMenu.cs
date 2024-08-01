using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool paused;
    public PlayerMovement pm;
    PlayerInputHandler ih;
    public PauseTabGroup tabGroup;

    private void Start()
    {
        
        paused = false;
        ih = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputHandler>();

        //if (ih != null)
        //{
        //    ih.InputMaster.
        //}
    }

    private void Update()
    {
        //look for inputs for the pause (esc., controller start button)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Alpha2))
        {
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
    }

    public void PauseGame()
    {
        //deactivate sub menus
        
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pm.canMove = false; //movement and looking off
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tabGroup.defaultTab.Select(); //set tab group to page 1
        
        pm.canMove = true; //movement and looking on
    }
}
