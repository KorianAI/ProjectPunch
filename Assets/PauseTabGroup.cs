using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseTabGroup : MonoBehaviour
{
    public List<PauseTabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public PauseTabButton selectedTab;
    public List<GameObject> objectsToSwap;

    public void Subscribe(PauseTabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<PauseTabButton>();
        }

        tabButtons.Add(button);
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
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }
        
        selectedTab = button;

        selectedTab.Select();

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
        foreach(PauseTabButton button in tabButtons)
        {
            if(selectedTab!=null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }
}
