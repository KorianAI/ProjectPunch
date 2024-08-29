using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ScrollBarSelecting : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Color selected;
    public Color notSelected;
    public Image handle;

    

    public void OnSelect(BaseEventData eventData)
    {
        handle.color = selected;
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        handle.color = notSelected;
    }
}
