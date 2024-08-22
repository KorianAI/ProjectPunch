using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MenuTutorialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TutorialViewer tutViewer;
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tutViewer.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tutViewer.OnTabExit(this);
    }

    public void OnSelect(BaseEventData eventData)
    {
        tutViewer.OnTabEnter(this);

        UpdateScrollbar();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        tutViewer.OnTabExit(this);
    }

    public void Select()
    {
        if (onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (onTabDeselected != null)
        {
            onTabDeselected.Invoke();
        }
    }

    private void UpdateScrollbar()
    {
        //int index = System.Array.IndexOf(tutViewer.tutButtons.ToArray(), gameObject);
        int index = gameObject.transform.GetSiblingIndex();

        if (index >= 0)
        {
            float normalizedPosition = (float)index / (tutViewer.tutButtons.ToArray().Length - 1);
            tutViewer.scrollRect.verticalNormalizedPosition = 1 - normalizedPosition; // Reverse it for the scrollbar
        }
    }
}
