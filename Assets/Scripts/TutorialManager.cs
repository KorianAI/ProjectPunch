using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance { get; private set; }

    public PlayerMovement pm;
    public PlayerStateManager sm;
    public CombatManager cm;

    public GameObject currentTutorial; //set current so that the appear/disappear can be played by just calling for the current one
    Animation currentAnim;

    bool startCombatAfter;

    public GameObject resourcesUI;

    public bool isTip;
    public float tipTimer = 5f;
    public float tipDelayTimer = 3f;

    //when player walks through trigger box
    //find the correct tut to show
    //play appear animation
    //disable player movement and looking
    //when A/Space pressed, play disappear anim and restore player movement

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
        InputMapManager.inputActions.Menus.ClosePopup.started += ctx =>
        {
            HideTutorial();
        };
    }

    public void SetCurrent(GameObject newTut, bool canWalk, bool tip, float tipDelay)
    {
        if (currentAnim)
        {
            HideTutorial();
            StopAllCoroutines();
            TurnOff();
        }

        currentTutorial = newTut;
        currentAnim = currentTutorial.GetComponentInChildren<Animation>();

        if (!sm.tutIsActive && !GameSettings.instance.skipTutorials)
        {
            ShowTutorial(canWalk);
        }

        if (tip)
        {
            isTip = tip;
            tipDelayTimer = tipDelay;

            StartCoroutine(HideTip());
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

    public void ShowTutorial(bool canWalk)
    {
        Debug.Log("bro");
        currentTutorial.SetActive(true);
        currentAnim.Play("TutorialWindowAppear");
        sm.tutIsActive = true;

        GameSettings.instance.walkDuringTutorials = canWalk;

        if (!canWalk)
        {
            InputMapManager.ToggleActionMap(InputMapManager.inputActions.Menus);
        }
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
            GameSettings.instance.walkDuringTutorials = false;
            isTip = false;

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

        SetCurrent(currentTutorial.GetComponent<TutorialNextPage>().pageToFollow, GameSettings.instance.walkDuringTutorials, isTip, 0f);
        currentTutorial.SetActive(true);
    }

    IEnumerator HideTip()
    {
        yield return new WaitForSeconds(tipDelayTimer);
        yield return new WaitForSeconds(tipTimer);
        HideTutorial();
    }
}
