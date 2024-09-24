using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    TutorialManager manager;
    public GameObject tutorialAnim;
    BoxCollider boxCollider;

    public bool startCombatAfter;
    public bool ableToMove;
    public bool tip;
    public float tipDelay;

    private void Start()
    {
        manager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerStateManager.instance.tutIsActive)
        {
            manager.SetCurrent(tutorialAnim, ableToMove, tip, tipDelay); //set the chosen tutorial object
            boxCollider.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerStateManager.instance.tutIsActive)
        {
            manager.SetCurrent(tutorialAnim, ableToMove, tip, tipDelay); //set the chosen tutorial object
            boxCollider.enabled = false;
        }
    }

    public void ActivateTut(CombatManager combatManager)
    {
        manager.SetCurrent(tutorialAnim, ableToMove, tip, tipDelay); //set the chosen tutorial object
        manager.PrepareForCombat(startCombatAfter, combatManager); //sets up combat to be played after the tut is closed
        boxCollider.enabled = false;
    }
}