using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StreetLamps : MonoBehaviour, IMagnetisable
{
    public GameObject playerObj;
    public Transform pullPos;

    private void Start()
    {
        playerObj = PlayerStateManager.instance.gameObject;
    }

    public void Pull(PlayerStateManager player)
    {
        player.speedlines.SetActive(true);
        player.resources.invincible = true;
        playerObj.transform.DOMove(pullPos.transform.position, 1.5f). OnComplete(() => { player.speedlines.SetActive(false); player.resources.invincible = false; player.canAttack = true; });
        playerObj.GetComponent<TargetLock>().currentTarget = null;
        playerObj.GetComponent<TargetLock>().isTargeting = false;
        DOTween.To(() => player.playerCam.m_Lens.FieldOfView, x => player.playerCam.m_Lens.FieldOfView = x, 50, .25f);
    }

    public void Push(PlayerStateManager player)
    {
        //Should play an audio effect to indicate that this does not work on static objects
        Debug.Log("nuh huh");
    }
}
