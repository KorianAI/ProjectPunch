using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetVisibilityCheck : MonoBehaviour
{
    public void OnBecameVisible()
    {
        if (!TargetManager.instance.targets.Contains(transform.parent.parent.gameObject))
        {
            TargetManager.instance.targets.Add(transform.parent.parent.gameObject);
        }
    }

    public void OnBecameInvisible()
    {
        if (TargetManager.instance.targets.Contains(transform.parent.parent.gameObject))
        {
            TargetManager.instance.targets.Remove(transform.parent.parent.gameObject);
        }
    }
}
