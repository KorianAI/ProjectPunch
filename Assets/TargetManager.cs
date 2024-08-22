using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<GameObject> targets;
}
