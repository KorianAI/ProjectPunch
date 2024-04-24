using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource source;

    [SerializeField] AudioClip startingTrack;
    
    public float timeToFade = 3;
    public float silenceTime = 5; //should be greater than time to fade

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
}
