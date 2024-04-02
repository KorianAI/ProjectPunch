using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlot : MonoBehaviour
{
    public Sprite fullSlot, halfSlot, emptySlot;
    public Image slotImage;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }
    
    public void SetSlotImage(SlotStatus status)
    {

        switch (status)
        {
            case SlotStatus.Empty:
                slotImage.sprite = emptySlot;
                break;
            case SlotStatus.Half:
                slotImage.sprite = halfSlot;
                break;
            case SlotStatus.Full:
                slotImage.sprite = fullSlot;
                break;
        }

    }


}

public enum SlotStatus
{
    Empty = 0,
    Half = 5,
    Full = 10
}