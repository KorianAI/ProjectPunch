using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ComboDisplay : MonoBehaviour
{
    public TMP_Text comboText;
    public float comboTimeWindow = 2f;

    private Dictionary<string, string> buttonNameMappings = new Dictionary<string, string>
    {
        { "buttonWest", "X" },
        { "buttonNorth", "Y" },
        // Add more mappings as needed for other buttons
    };

    private List<string> comboList = new List<string>();
    private float lastInputTime = 0f;
    private bool isWaitingForInput = false;

    private void OnEnable()
    {
        InputMaster comboActions = new InputMaster();
        comboActions.Enable();

        // Register callbacks for each action
        comboActions.Player.LightAttack.started += ctx => OnComboActionStarted(ctx.control.name);
        comboActions.Player.HeavyAttack.started += ctx => OnComboActionStarted(ctx.control.name);
        comboActions.Player.Movement.started += ctx => OnComboActionStarted(ctx.control.name);
    }

    private void OnDisable()
    {
        InputMaster comboActions = new InputMaster();
        comboActions.Disable();

        // Clear combo list when disabled
        comboList.Clear();
        UpdateComboText();
    }

    private void OnComboActionStarted(string buttonName)
    {
        if (buttonNameMappings.ContainsKey(buttonName))
        {
            buttonName = buttonNameMappings[buttonName];
        }

        // If waiting for input, reset timer and add to combo list
        if (isWaitingForInput)
        {
            comboList.Add(buttonName);
            lastInputTime = Time.time;
        }
        else
        {
            // Start a new combo sequence
            comboList.Clear();
            comboList.Add(buttonName);
            lastInputTime = Time.time;
            isWaitingForInput = true;
            Invoke(nameof(ClearCombo), comboTimeWindow);
        }

        // Update combo text immediately
        UpdateComboText();
    }

    void ClearCombo()
    {
        // Clear combo after time window if no new input received
        isWaitingForInput = false;
        comboList.Clear();
        UpdateComboText();
    }

    void UpdateComboText()
    {
        comboText.text = string.Join(", ", comboList.ToArray());
    }
}
