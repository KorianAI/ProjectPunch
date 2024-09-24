using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class Respawn : MonoBehaviour
{
    [Header("References")]
    PlayerStateManager sm;
    PlayerResources pr;
    public GameObject respawnAnimObj;
    Animation reloadAnim;
    CheckpointManager cpManager;

    [Header("Variables")]
    bool canReset = true;
    bool respawnActive = false;
    public float fallThreshold = -5f;
    
    private void Start()
    {
        sm = GetComponent<PlayerStateManager>();
        pr = GetComponent<PlayerResources>();
        if (respawnAnimObj != null )
        {
            reloadAnim = respawnAnimObj.GetComponent<Animation>();
        }
        cpManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
    }

    void LateUpdate()
    {
        if (transform.position.y < fallThreshold && canReset)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
            // ^ nuclear option in case this script doesn't work
            Debug.Log("bruhbruhbruh");
            ResetPlayer();
        }
    }

    public void ResetPlayer()
    {
        sm.SwitchState(sm.idleState);
        sm.pm.yVelocity = 0;
        sm.pm.velocity = Vector3.zero;
        canReset = false;
        sm.tl.CancelLock();

        if (cpManager != null)
        {
            reloadAnim.Play();
        }
    }

    public void RespawnAppear()
    {
        if (!respawnActive)
        {
            respawnActive = true;

            //set player position
            if (cpManager.respawnPoint != null)
            {
                transform.DOMove(cpManager.respawnPoint.position, .01f);
            }

            else
            {
                transform.DOMove(cpManager.defaultStartPoint.transform.position, .01f);
            }

            pr.ReplenishAll();
        }
    }

    public void RespawnDisappear() //should be setting things back to how they were
    {
        respawnActive = false;
        StartCoroutine(AllowRespawn());
    }

    IEnumerator AllowRespawn() //delayed reset of the canReset bool
    { 
        yield return new WaitForSeconds(1f);
        canReset = true;
    }    
}
