using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HealthBars : MonoBehaviour
{
    CinemachineBrain brain;

    float timer;
    float maxTime = 3f;
    bool addToTimer;

    Canvas canvas;

    bool lockedOn;

    private void Start()
    {
        brain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
        canvas = GetComponent<Canvas>();

        HideBars();
    }

    private void LateUpdate()
    {
        transform.LookAt(brain.OutputCamera.transform, Vector3.up);

        //when player attacks or locks onto enemy, should make the things appear
        //after they end lock on/after attacking, the timer should start
        //when maxTime reached, disable the things


        if (addToTimer && !lockedOn)
        {
            timer += Time.deltaTime;

            if (timer >= maxTime)
            {
                addToTimer = false;
                timer = 0f;

                HideBars();
            }
        }
    }

    public void ShowBars()
    {
        addToTimer = false;
        canvas.enabled = true;
        lockedOn = true;
    }

    public void ShowBarsAttacked()
    {
        addToTimer = true;
        canvas.enabled = true;
        timer = 0f;
    }

    public void ShowBarsTargeted()
        {
            addToTimer = true;
            lockedOn = false;
        }

    public void HideBars()
    {
        canvas.enabled = false;
    }
}
