using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }

    public AudioSource source;

    [SerializeField] AudioClip startingTrack;
    
    public float timeToFade = 3;
    public float silenceTime = 5; //should be greater than time to fade

    public bool fighting = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(SwapTrack(startingTrack));
    }    

    public IEnumerator SwapTrack(AudioClip newMusic)
    {
        float timeElapsed = 0;
        
        //fade old track
        while (timeElapsed < timeToFade)
        {
            source.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);

            timeElapsed += Time.deltaTime;
        }
        
        yield return new WaitForSeconds(0);

        source.Stop();

        source.volume = 1f;
        source.loop = true;
        source.clip = newMusic;
        source.Play();
    }

    public void ToggleEnemiesActive(bool value)
    {
        fighting = value;
    }
}
