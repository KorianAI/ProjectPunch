using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public float maxValue;
    public float currentValue;
    List<UISlot> slots = new List<UISlot>();


    private void Start()
    {
        Invoke("DrawSlots", .01f);
    }

    public void DrawSlots()
    {
        ClearSlots();

        float maxHealthRemainder = maxValue % 10;
        int slotsToMake = (int)((maxValue / 10f) + maxHealthRemainder);

        for (int i = 0; i < slotsToMake; i++)
        {
            CreateEmptySlot();
        }

        for (int i = 0; i < slots.Count; i++)
        {
            int slotStatusRemainder = (int)Mathf.Clamp(currentValue - (i * 10), 0, 10);
            slots[i].SetSlotImage((SlotStatus)slotStatusRemainder);
        }
    }

    public void CreateEmptySlot()
    {
        GameObject newSlot = Instantiate(slotPrefab, transform, false);
        newSlot.transform.localScale = new Vector3(1, 1, 1);
        newSlot.transform.SetParent(transform);

        UISlot slotComponent = newSlot.GetComponent<UISlot>();
        slotComponent.SetSlotImage(SlotStatus.Empty);
        slots.Add(slotComponent);
    }

    public void ClearSlots()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        slots = new List<UISlot>();
    }
}
