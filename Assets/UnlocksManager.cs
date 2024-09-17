using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlocksManager : MonoBehaviour
{
    public PauseTabGroup group;

    [Header("Scrolls")]
    public List<GameObject> scrolls;
    public Scrollbar scrollsBar;
    public int lowestScroll;

    private void Start()
    {
        foreach (GameObject scrollObj in scrolls)
        {
            scrollObj.SetActive(false);
            lowestScroll = scrolls.Count;
        }
    }

    public void UnlockScroll(int scrollNo)
    {
        foreach (GameObject scrollButton in scrolls)
        {
            int index = scrollButton.transform.GetSiblingIndex();

            if (scrollNo == index)
            {
                scrollButton.SetActive(true);
            }

            if (scrollNo < lowestScroll)
            {
                //when unlocking
                //if unlock is lowest in list
                //set scrollbar 'select on left' to that scroll button
                //allows me to always move to the lowest possible option

                lowestScroll = scrollNo;

                //Create a new navigation
                Navigation NewNav = new Navigation();
                NewNav.mode = Navigation.Mode.Explicit;

                //Set what you want to be selected on down, up, left or right;
                NewNav.selectOnLeft = scrolls[scrollNo].GetComponent<Button>();

                //Assign the new navigation to your desired button or ui Object
                scrollsBar.GetComponent<Scrollbar>().navigation = NewNav;
                group.firstSelected[1] = scrolls[scrollNo].gameObject; //change the first selected object for that section
            }
        }
    }
}