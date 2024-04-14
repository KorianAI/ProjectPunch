using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject slotPrefab;
    public float maxHealth;
    public float currentHealth;
    List<HealthSlot> slots = new List<HealthSlot>();


    private void Start()
    {
        Invoke("DrawSlots", .01f);
    }

    public void DrawSlots()
    {
        ClearSlots();

        float maxHealthRemainder = maxHealth % 10;
        int slotsToMake = (int)((maxHealth / 10f) + maxHealthRemainder);

        for (int i = 0; i < slotsToMake; i++)
        {
            CreateEmptySlot();
        }

        for (int i = 0; i < slots.Count; i++)
        {
            int slotStatusRemainder = (int)Mathf.Clamp(currentHealth - (i * 10), 0, 10);
            slots[i].SetSlotImage((SlotStatus)slotStatusRemainder);
        }
    }

    public void CreateEmptySlot()
    {
        GameObject newSlot = Instantiate(slotPrefab, transform, false);
        newSlot.transform.localScale = new Vector3(1, 1, 1);
        newSlot.transform.SetParent(transform);

        HealthSlot slotComponent = newSlot.GetComponent<HealthSlot>();
        slotComponent.SetSlotImage(SlotStatus.Empty);
        slots.Add(slotComponent);
    }

    public void ClearSlots()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        slots = new List<HealthSlot>();
    }
}
