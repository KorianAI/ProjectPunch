using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenAudio : MonoBehaviour
{

    [Header("Main Theme")]
    public AK.Wwise.Event playMusic_MainTheme;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        //if (!HasPlayedMusic.instance.hasPlayedMusic)
        //{
            
        //    HasPlayedMusic.instance.hasPlayedMusic = true;
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MusicStart()
    {

        Music_MainTheme();
    }

    public void Music_MainTheme()
    {
        playMusic_MainTheme.Post(gameObject);
    }
}
