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
    public int currentIndex = 0;

    //script used in conjunction with PauseTabButton script
    //assigns tabs from list to the buttons in the UI, and allows for swapping beterrn them
    //changes the images used on each button to show whether they've been selected, hovered over or deselected

    private void Start()
    {
        Invoke("SelectDefault", 0.01f);
    }

    void SelectDefault()
    {
        OnTabSelected(defaultTab);
        Debug.Log("invoked tab default");
        pm.pauseMenu.SetActive(false);
    }



    private void Update()
    {
        if (pm.paused) //only allows tab swapping if paused
        {
            int index = selectedTab.transform.GetSiblingIndex(); //find index of currentTab in tabButtons list
            Debug.Log(index);

            if (Input.GetKeyDown(KeyCode.Alpha1)) //swap tab left
            {
                if (index >= 0 && index < (tabButtons.Count))
                {
                    //index -1
                    //index--;

                    OnTabSelected(tabButtons[index -1]);
                }

                if (index <= 0)
                {
                    //set to highest in tabButtons
                    //index = (tabButtons.Count - 1);
                    
                    OnTabSelected(tabButtons[tabButtons.Count - 1]);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3)) //swap tab right
            {
                if (index >= 0 && index < (tabButtons.Count))
                {
                    //index +1
                    //index++;

                    OnTabSelected(tabButtons[index + 1]);
                }

                if (index >= (tabButtons.Count))
                {
                    //set to lowest in tabButtons
                    //index = 0;

                    OnTabSelected(tabButtons[0]);
                }
            }


            //if (Input.GetKeyDown(KeyCode.Alpha4)) //swap tab left
            //{   
            //    for (int i = 0; i < tabButtons.Count; i++)
            //    {
            //        if (i == index)
            //        {
            //            if (i <= 0)
            //            {
            //                //set to highest in tabButtons
            //                i = tabButtons.Count;
            //                //tabButtons[i].Select();
            //                Debug.Log(tabButtons[i]);
            //                OnTabSelected(tabButtons[i]);
            //            }
            //            else if (i >= 0)
            //            {
            //                //index -1
            //                i--;
            //                //tabButtons[i].Select();
            //                Debug.Log(tabButtons[i]);

            //                OnTabSelected(tabButtons[i]);

            //            }
            //        }
            //    }
            //}

            //if (Input.GetKeyDown(KeyCode.Alpha5)) //swap tab right
            //{
            //    for (int i = 0; i < tabButtons.Count; i++)
            //    {
            //        if (i == index)
            //        {
            //            if (i >= tabButtons.Count)
            //            {
            //                //set to lowest in tabButtons
            //                i = 0;
            //                //tabButtons[i].Select();
            //                Debug.Log(tabButtons[i]);

            //                OnTabSelected(tabButtons[i]);
            //            }
            //            else if (i >= 0)
            //            {
            //                //index +1
            //                i++;
            //                //tabButtons[i].Select();
            //                Debug.Log(tabButtons[i]);

            //                OnTabSelected(tabButtons[i]);
            //            }
            //        }
            //    }
            //}
        }
    }

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
