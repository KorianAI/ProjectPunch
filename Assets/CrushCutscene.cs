using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CrushCutscene : MonoBehaviour
{
    public PlayableDirector dir;
    public GameObject player;
    public GameObject cutCam1;
    public GameObject cutCam2;
    BoxCollider box;

    private void Start()
    {
        box = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dir.Play();
            player = other.gameObject;
            player.SetActive(false);
        }
    }

    public void End()
    {
        Debug.Log("cutscene ended");
        player.SetActive(true);

        //cutCam1.SetActive(false);
        //cutCam2.SetActive(false);

        box.enabled = false;
        Debug.Log("activated bits");
    }
}
