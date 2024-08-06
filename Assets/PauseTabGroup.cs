using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseTabGroup : MonoBehaviour
{
    public PauseMenu pm;
    [Space]
    public List<PauseTabButton> tabButtons;
    [Space]
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    [Space]
    public PauseTabButton selectedTab;
    public PauseTabButton defaultTab;
    [Space]
    public List<GameObject> objectsToSwap;
    //public int index = 0;

    //script used in conjunction with PauseTabButton script
    //assigns tabs from list to the buttons in the UI, and allows for swapping beterrn them
    //changes the images used on each button to show whether they've been selected, hovered over or deselected

    private void Start()
    {
        Invoke("SetInitialTab", 0.5f);
        
        InputMapManager.inputActions.Menus.TabLeft.started += ctx =>
        {
            TabLeft();
        };

        InputMapManager.inputActions.Menus.TabRight.started += ctx =>
        {
            TabRight();
        };
    }

    private void SetInitialTab()
    {
        OnTabSelected(defaultTab);
        
        if (pm.pauseUI.activeSelf == true)
        {
            pm.pauseUI.SetActive(false);
        }
    }

    private void Update()
    {
        //index = selectedTab.transform.GetSiblingIndex(); //find index of currentTab in tabButtons list
    }

    public void Subscribe(PauseTabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<PauseTabButton>();
        }

        tabButtons.Add(button);
    }

    public void TabLeft() //swap tab left
    {
        if (selectedTab.transform.GetSiblingIndex() > 0)
        {
            //index--;
            OnTabSelected(tabButtons[selectedTab.transform.GetSiblingIndex() -1]);
        }

        else if (selectedTab.transform.GetSiblingIndex() <= 0)
        {
            //set to highest in tabButtons
            OnTabSelected(tabButtons[tabButtons.Count -1]);
        }
    }

    public void TabRight() //swap tab right
    {
        if (selectedTab.transform.GetSiblingIndex() < (tabButtons.Count -1))
        {
            //index++;
            OnTabSelected(tabButtons[selectedTab.transform.GetSiblingIndex() +1]);
        }

        else if (selectedTab.transform.GetSiblingIndex() >= (tabButtons.Count -1))
        {
            //set to lowest in tabButtons
            OnTabSelected(tabButtons[0]);
        }
    }

    public void OnTabEnter(PauseTabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(PauseTabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(PauseTabButton button)
    {
        if(selectedTab != null) //deselects currently selected tab
        {
            selectedTab.Deselect();
        }
        
        selectedTab = button; //assigns new selected tab

        selectedTab.Select(); //selects new tab

        ResetTabs();
        button.background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for(int i=0; i<objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(PauseTabButton button in tabButtons) //looks through all tabs
        {
            if(selectedTab!=null && button == selectedTab) { continue; } //skips currently selected tab
            button.background.sprite = tabIdle; //sets all other tabs to idle appearance
        }
    }
}
