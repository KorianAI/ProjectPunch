using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loading;
    
    private void Start()
    {
        loading.SetActive(false);
    }

    public void ChangeScene(int sceneId)
    {
        //make the screen fade to black first
        loading.SetActive(true);

        StartCoroutine(Load(sceneId));
    }    

    IEnumerator Load(int sceneId)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneId);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
