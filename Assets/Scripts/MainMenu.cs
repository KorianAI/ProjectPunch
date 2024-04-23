using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void ChangeScene(int sceneId)
    {
        //make the screen fade to black first?
        
        SceneManager.LoadScene(sceneId);
    }    

    public void Quit()
    {
        Application.Quit();
    }
}
