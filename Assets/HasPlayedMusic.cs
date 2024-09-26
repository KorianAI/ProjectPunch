using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HasPlayedMusic : MonoBehaviour
{
    [Header("Main Theme")]
    public AK.Wwise.Event playMusic_MainTheme;

    public static HasPlayedMusic instance;
    public bool hasPlayedMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

 

    }


    private void Start()
    {
        Music_MainTheme();
    }



    public void Music_MainTheme()
    {
        playMusic_MainTheme.Post(gameObject);
    }
}
