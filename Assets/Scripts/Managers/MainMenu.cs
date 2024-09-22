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
        if (loading != null)
        {
            loading.SetActive(false);
        }
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha0))
        {
            ChangeScene(3);
        }
    }

    public void ChangeScene(int sceneId)
    {
        //make the screen fade to black first
        if (loading != null)
        {
            loading.SetActive(true);
        }

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
