using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
    [SerializeField] ExtendoArms extArms;
    public void TriggerShockwave(float value)
    {
        if (extArms != null)
        {
            extArms.Shockwave(value);
        }
    }
}
