using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseControls : MonoBehaviour
{
    public PauseMenu pauseScript;

    private void Start()
    {
        pauseScript = GetComponentInParent<PauseMenu>();
    }

    public void PauseGame()
    {
        pauseScript.PauseGame();
    }

    public void ResumeGame()
    {
        pauseScript.ResumeGame();
    }

    public void ChangeScene(int sceneId)
    {
        Time.timeScale = 1;
        HasPlayedMusic.instance.Music_MainTheme();
        SceneManager.LoadScene(sceneId);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
