using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPrototype : MonoBehaviour
{
    public void LoadNextLevel()
    {
       
        SceneManager.LoadScene(2);
        
    }
}
