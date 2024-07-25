using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommand
{
    public InputType Type;
    public Vector2 Direction;
    public float DisplayTime = 2; // Remaining display time for this input
    public override string ToString()
    {
        string directionText = Type == InputType.Y ? $" {Direction.ToString()}" : "";
        return $"{Type.ToString()}{directionText}";
    }
}

public enum InputType
{
    X,
    xH,
    Y,
    yH,
    A,
    B,
    Push,
    Pull
}
