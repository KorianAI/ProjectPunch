using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialViewer : MonoBehaviour
{
    public PauseMenu pm;
    [Space]
    public List<MenuTutorialButton> tutButtons;
    [Space]
    public MenuTutorialButton selectedButton;
    [Space]
    public List<GameObject> tutorials;

    public Color selected;
    public Color notSelected;
    public ScrollRect scrollRect;

    public void OnTabEnter(MenuTutorialButton button)
    {
        if (selectedButton != null) //deselects currently selected button
        {
            selectedButton.Deselect();
        }

        selectedButton = button; //assigns new selected button
        selectedButton.Select(); //selects new button

        button.GetComponent<Image>().color = selected;

        ResetTabs();

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < tutorials.Count; i++)
        {
            if (i == index)
            {
                tutorials[i].SetActive(true);
            }
            else
            {
                tutorials[i].SetActive(false);
            }
        }
    }

    public void OnTabExit(MenuTutorialButton button)
    {
        button.GetComponent<Image>().color = notSelected;
    }

    public void ResetTabs()
    {
        foreach (MenuTutorialButton button in tutButtons) //looks through all tabs
        {
            if (selectedButton != null && button == selectedButton) { continue; } //skips currently selected tab
        }
    }
}
