using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetVisibilityCheck : MonoBehaviour //exists in same location as obj's mesh renderer, as that is what becomes visible to the cameras
{
    public void OnBecameVisible()
    {
        if (GetComponent<Targetable>()) //check if this object has the targetable component
        {
            if (!TargetManager.instance.targets.Contains(transform.gameObject))
            {
                TargetManager.instance.targets.Add(transform.gameObject);
            }
        }
        else if (transform.parent.gameObject.GetComponent<Targetable>()) //check if this object's parent has the targetable component
        {
            if (!TargetManager.instance.targets.Contains(transform.parent.gameObject))
            {
                TargetManager.instance.targets.Add(transform.parent.gameObject);
            }
        }
        else if (transform.parent.parent.gameObject.GetComponent<Targetable>()) //check if this object's grandparent has the targetable component
        {
            if (!TargetManager.instance.targets.Contains(transform.parent.parent.gameObject))
            {
                TargetManager.instance.targets.Add(transform.parent.parent.gameObject);
            }
        }
    }

    public void OnBecameInvisible()
    {
        if (GetComponent<Targetable>())
        {
            if (TargetManager.instance.targets.Contains(transform.gameObject))
            {
                TargetManager.instance.targets.Remove(transform.gameObject);
            }
        }
        else if (transform.parent.gameObject.GetComponent<Targetable>())
        {
            if (TargetManager.instance.targets.Contains(transform.parent.gameObject))
            {
                TargetManager.instance.targets.Remove(transform.parent.gameObject);
            }
        }
        else if (transform.parent.parent.gameObject.GetComponent<Targetable>())
        {
            if (TargetManager.instance.targets.Contains(transform.parent.parent.gameObject))
            {
                TargetManager.instance.targets.Remove(transform.parent.parent.gameObject);
            }
        }
    }
}

//this obj (which has the mesh/skinned mesh renderer) may not contain the targetable script
//this script has to exist on the renderer, as that is what appears as visible to the cameras

//therefore:
//look to see if this object has a targetable script
//if so, then check target manager for this obj
//if not, check parent, then repeat with grandparent
