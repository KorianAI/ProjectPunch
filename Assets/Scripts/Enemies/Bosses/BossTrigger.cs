using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public AK.Wwise.Event playMusic_Boss1;
    public BossInfo boss;
    BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.StartFight();
            //PlayMusic_Boss1();
            //MusicManager.instance.ToggleFightingBoss(true);
            PlayerStateManager.instance.inBossFight = true;
            PlayerStateManager.instance.resources.ChangeGauntlets(1);
            PlayerStateManager.instance.resources.PlayMusic_Boss1();
            boxCollider.enabled = false;
        }
    }

}
