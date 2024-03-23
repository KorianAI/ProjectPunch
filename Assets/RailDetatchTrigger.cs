using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailDetatchTrigger : MonoBehaviour
{
    public GameObject playerObj;
    public PlayerStateManager ps;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            playerObj = other.gameObject;
            ps = playerObj.GetComponent<PlayerStateManager>();

            if (ps.currentState == ps.railState)
            {
                playerObj.GetComponent<TargetLock>().currentTarget = null;
                playerObj.GetComponent<TargetLock>().isTargeting = false;
                playerObj.GetComponent<TargetLock>().lastTargetTag = null;

                playerObj.transform.SetParent(null);

                ps.cam.ChangeCam(false);

                ps.SwitchState(ps.inAirState);

                ps.anim.Play("PlayerInAir");
                ps.anim.SetBool("onRail", false);
            }           
        }
    }
}
