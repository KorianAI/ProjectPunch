using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CrushCutscene : MonoBehaviour
{
    public PlayableDirector dir;
    public PlayableAsset CutsceneP1;
    public PlayableAsset CutsceneP2;

    public GameObject player;
    public GameObject cutCam1;
    public GameObject cutCam2;
    BoxCollider box;

    public Animation RTPress;
    public int pressAmt = 8;
    public int currentAmt = 0;
    bool p1Ended = false;
    bool playedP2 = false;

    public Animator kailaAnim;

    bool skipCheck;
    public GameObject skipUI;

    public GameObject kailaCutscene;
    public GameObject pole;
    public Vector3 poleEndPos;
    public Vector3 poleEndRot;
    public GameObject tank;
    public Vector3 tankEndPos;
    public Vector3 tankEndRot;

    private void Start()
    {
        box = GetComponent<BoxCollider>();
        skipUI.SetActive(false);

        InputMapManager.inputActions.Cutscene.Push.performed += ctx =>
        {
            Push();
        };

        InputMapManager.inputActions.Cutscene.Skip.started += ctx =>
        {
            Skip();
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dir.Play(CutsceneP1);
            player = other.gameObject;
            player.SetActive(false);
            InputMapManager.ToggleActionMap(InputMapManager.inputActions.Cutscene);
        }
    }

    public void P1Ended()
    {
        p1Ended = true;
        RTPress.gameObject.SetActive(true);
        RTPress.Play();
    }

    public void Push()
    {
        kailaAnim.Play("Push");

        if (currentAmt >= pressAmt && p1Ended && !playedP2)
        {
            StartP2();
        }

        else if (currentAmt < pressAmt && p1Ended && !playedP2)
        {
            currentAmt++;
        }
    }

    void StartP2()
    {
        playedP2 = true;
        RTPress.gameObject.SetActive(false);
        dir.Play(CutsceneP2);
    }

    public void End()
    {
        InputMapManager.ToggleActionMap(InputMapManager.inputActions.Player);
        player.SetActive(true);

        //cutCam1.SetActive(false);
        //cutCam2.SetActive(false);

        box.enabled = false;
    }

    public void Skip()
    {
        if (!skipCheck)
        {
            skipCheck = true;
            skipUI.SetActive(true);
        }

        else
        {
            if (!p1Ended && !playedP2) //if p1, get to the mash bit
            {
                P1Ended();
                dir.Stop();

                cutCam1.SetActive(false);
                cutCam2.SetActive(false);

                skipCheck = false;
                skipUI.SetActive(false);
                Debug.Log("skipped 1");
            }

            else if (p1Ended && !playedP2) //if mash, start p2
            {
                StartP2();

                skipCheck = false;
                skipUI.SetActive(false);
                Debug.Log("skipped mash");
            }

            else if (p1Ended && playedP2) //if p2 end
            {
                End();
                dir.Stop();

                cutCam1.SetActive(false);
                cutCam2.SetActive(false);

                skipCheck = false;
                skipUI.SetActive(false);

                if(player != null)
                {
                    player.SetActive(true);
                }
                kailaCutscene.SetActive(false);

                //makes sure it all actually goes through to the end of the section:
                pole.transform.localPosition = poleEndPos;
                pole.transform.localRotation = Quaternion.Euler(poleEndRot.x, poleEndRot.y, poleEndRot.z);
                tank.transform.localPosition = tankEndPos;
                tank.transform.localRotation = Quaternion.Euler(tankEndRot.x, tankEndRot.y, tankEndRot.z);

                Debug.Log("skipped 2");
            }
        }
    }
}
