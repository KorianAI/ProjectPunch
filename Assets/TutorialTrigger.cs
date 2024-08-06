using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    TutorialManager manager;
    public GameObject tutorialAnim;
    BoxCollider boxCollider;

    private void Start()
    {
        manager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.SetCurrent(tutorialAnim); //set the chosen tutorial object
            boxCollider.enabled = false;
        }
    }

    public void ActivateTut()
    {
        manager.SetCurrent(tutorialAnim); //set the chosen tutorial object
        boxCollider.enabled = false;
    }
}
