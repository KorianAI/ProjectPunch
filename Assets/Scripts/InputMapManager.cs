using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputMapManager : MonoBehaviour
{
    public static InputMaster inputActions;
    public static event Action<InputActionMap> actionMapChange;

    private void Awake()
    {
        inputActions = new InputMaster();
    }

    void Start()
    {
        //start with the player controller enabled
        ToggleActionMap(inputActions.Player);
    }

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled) { return; }

        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();

        //Debug.Log("New Action Map: " + actionMap.name);
    }
}
