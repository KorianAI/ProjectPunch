using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool paused = false;
    public PlayerMovement pm;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        //look for inputs for the pause (esc., controller start button)
        if (Input.GetKeyDown(KeyCode.Escape))
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

        pm.canMove = false; //movement and looking off
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        pm.canMove = true; //movement and looking on
    }

    public void ShowCombos()
    {

    }

    public void HideCombos()
    {

    }

    public void ChangeScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
