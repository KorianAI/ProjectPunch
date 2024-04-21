using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public bool canReset = true;
    public bool fadedIn = false;
    public bool fadedOut = false;

    PlayerStateManager sm;


    private void Start()
    {
        followTarget = deathCam.Follow.gameObject;
        sm = GetComponent<PlayerStateManager>();
        vignetteAnim = vignette.GetComponent<Animator>();
        vignette.SetActive(false);

        cpManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
    }

    void LateUpdate()
    {
        if (transform.position.y < threshold && canReset)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // <- nuclear option in case this doesn't work

            sm.SwitchState(sm.idleState);
            sm.yVelocity = 0;
            sm.velocity = Vector3.zero;
            canReset = false;

            if (cpManager != null)
            {
                ResetPlayer();
            }
        }
    }

    void ResetPlayer()
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
            Debug.Log("Respawn fading out");

            vignette.SetActive(true);
            vignetteAnim.SetTrigger("FadeOut");
        }
    }

    public void FadeIn() //removes vignette
    {
        if (!fadedIn)
        {
            fadedIn = true;
            Debug.Log("Respawn fading in");

            //set player position --THIS IS THE BIT THAT WORKS AND THEN DOESN'T
            if (cpManager.respawnPoint != null)
            {
                transform.DOMove(cpManager.respawnPoint.position, .01f);
            }

            vignetteAnim.SetTrigger("FadeIn");
        }
    }

    public void ResetCams() //should be setting things back to how they were
    {
        CameraManager.SwitchPlayerCam(PlayerStateManager.instance.playerCam);
        deathCam.Follow = followTarget.transform;
        vignette.SetActive(false);
        fadedOut = false;
        fadedIn = false;

        StartCoroutine(AllowRespawn());
    }

    IEnumerator AllowRespawn() //tried to perform a delayed reset of the canReset bool, but because the player gets moved back to the ground it gets set to false again on next frame
    { 
        yield return new WaitForSeconds(1f);
        Debug.Log("canreset");
        canReset = true;
    }

}
