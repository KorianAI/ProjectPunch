using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public PlayerMovement pm;
    public PlayerStateManager sm;

    public GameObject currentTutorial; //set current so that the appear/disappear can be played by just calling for the current one
    Animation currentAnim;
    public GameObject railTut;   

    public void SetCurrent(GameObject newTut)
    {
        currentTutorial = newTut;
        currentAnim = currentTutorial.GetComponentInChildren<Animation>();

        if (!sm.tutIsActive)
        {
            ShowTutorial();
        }
    }

    public void ShowTutorial()
    {
        currentTutorial.SetActive(true);
        pm.canMove = false; //movement and looking off
        currentAnim.Play("TutorialWindowAppear");
        sm.tutIsActive = true;
    }

    public void HideTutorial()
    {
        pm.canMove = true; //movement and looking on
        currentAnim.Play("TutorialWindowDisappear");
        sm.tutIsActive = false;
        //currentTutorial.SetActive(false);
    }


    //when player walks through trigger box
    //find the correct tut to show
    //play appear animation
    //disable player movement and looking
    //when A/Space pressed, play disappear anim and restore player movement
}
