using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class Respawn : MonoBehaviour
{
    public float threshold = -5f;
 
    public GameObject vignette;
    Animator vignetteAnim;

    public CinemachineVirtualCamera deathCam;
    GameObject followTarget;

    CheckpointManager cpManager;

    bool canReset = true;
    bool fadedIn = false;
    bool fadedOut = false;

    PlayerStateManager sm;
    PlayerResources pr;


    private void Start()
    {
        followTarget = deathCam.Follow.gameObject;
        sm = GetComponent<PlayerStateManager>();
        pr = GetComponent<PlayerResources>();
        vignetteAnim = vignette.GetComponent<Animator>();
        vignette.SetActive(false);

        cpManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
    }

    void LateUpdate()
    {
        if (transform.position.y < threshold && canReset)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
            // ^ nuclear option in case this script doesn't work

            sm.SwitchState(sm.idleState);
            sm.pm.yVelocity = 0;
            sm.pm.velocity = Vector3.zero;
            canReset = false;

            if (cpManager != null)
            {
                ResetPlayer();
            }
        }
    }

    public void ResetPlayer()
    {
        CameraManager.SwitchNonPlayerCam(deathCam);
        deathCam.Follow = null;
        FadeOut();
    }

    public void FadeOut() //adds vignette
    {
        if (!fadedOut)
        {
            fadedOut = true;

            vignette.SetActive(true);
            vignetteAnim.SetTrigger("FadeOut");
        }
    }

    public void FadeIn() //removes vignette
    {
        if (!fadedIn)
        {
            fadedIn = true;

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

            CameraManager.SwitchPlayerCam(PlayerStateManager.instance.playerCam);

            vignetteAnim.SetTrigger("FadeIn");
        }
    }

    public void ResetCams() //should be setting things back to how they were
    {
        deathCam.Follow = followTarget.transform;
        vignette.SetActive(false);
        fadedOut = false;
        fadedIn = false;

        StartCoroutine(AllowRespawn());
    }

    IEnumerator AllowRespawn() //delayed reset of the canReset bool
    { 
        yield return new WaitForSeconds(1f);
        canReset = true;
    }

}
