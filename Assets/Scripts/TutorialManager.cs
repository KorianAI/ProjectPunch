using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public PlayerMovement pm;
    public PlayerStateManager sm;
    public CombatManager cm;

    public GameObject currentTutorial; //set current so that the appear/disappear can be played by just calling for the current one
    Animation currentAnim;

    bool startCombatAfter;

    public GameObject resourcesUI;

    //when player walks through trigger box
    //find the correct tut to show
    //play appear animation
    //disable player movement and looking
    //when A/Space pressed, play disappear anim and restore player movement

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

        if (!sm.tutIsActive && !GameSettings.instance.skipTutorials)
        {
            ShowTutorial();
        }
    }

    public void PrepareForCombat(bool startCombat, CombatManager combatManager)
    {
        if (startCombat)
        {
            startCombatAfter = true;
        }

        cm = combatManager;
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
        if (sm.tutIsActive && currentTutorial.GetComponent<TutorialNextPage>().anotherPage && currentTutorial.GetComponent<TutorialNextPage>().pageToFollow != null) //skips to next tutorial, if available
        {
            NextPage();
        }
        
        else if (sm.tutIsActive)
        {
            currentAnim.Play("TutorialWindowDisappear");

            sm.tutIsActive = false;
            InputMapManager.ToggleActionMap(InputMapManager.inputActions.Player);

            if (startCombatAfter == true && cm != null) //starts combat if necessary. Requires the correct combat manager.
            {
                cm.StartCombat();
                startCombatAfter = false;
                cm = null;
            }

            StartCoroutine(TurnOff());
        }

        
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(1);

        if (currentTutorial != null)
        {
            currentTutorial.SetActive(false);
        }
    }

    public void NextPage() //allows another page to follow the previous tutorial
    {
        currentTutorial.SetActive(false);
        //currentTutorial = currentTutorial.GetComponent<TutorialNextPage>().pageToFollow;

        SetCurrent(currentTutorial.GetComponent<TutorialNextPage>().pageToFollow);
        currentTutorial.SetActive(true);
    }
}
