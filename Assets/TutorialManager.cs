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

    private void Start()
    {
        InputMapManager.inputActions.Menus.ClosePopup.started += ctx =>
        {
            HideTutorial();
        };
    }

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
        currentAnim.Play("TutorialWindowAppear");
        sm.tutIsActive = true;
        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Menus);
    }

    public void HideTutorial()
    {
        if (sm.tutIsActive)
        {
            currentAnim.Play("TutorialWindowDisappear");
        }
        sm.tutIsActive = false;
        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Player);

        StartCoroutine(TurnOff());
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(2);
        currentTutorial.SetActive(false);
    }

    //when player walks through trigger box
    //find the correct tut to show
    //play appear animation
    //disable player movement and looking
    //when A/Space pressed, play disappear anim and restore player movement
}
