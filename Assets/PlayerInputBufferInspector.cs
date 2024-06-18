using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerInputHandler))]
public class PlayerInputBufferInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerInputHandler playerInputManager = (PlayerInputHandler)target;

        // Display the buffer contents
        List<InputCommand> bufferedInputs = playerInputManager.GetBufferedInputsForInspector();
        if (bufferedInputs.Count > 0)
        {
            GUILayout.Label("Input Buffer:");
            foreach (var command in bufferedInputs)
            {
                GUILayout.Label($"Command Type: {command.Type}");
            }
        }
        else
        {
            GUILayout.Label("Input Buffer is empty");
        }
    }
}
