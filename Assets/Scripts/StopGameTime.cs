using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGameTime : MonoBehaviour
{
    public void TimeScaleZero()
    {
        Time.timeScale = 0;
    }

    public void TimeScaleOne()
    {
        Time.timeScale = 1;
    }
}
