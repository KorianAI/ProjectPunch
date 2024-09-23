using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public AK.Wwise.Event playMusic_Boss1;
    public BossInfo boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.StartFight();
            //PlayMusic_Boss1();
            //MusicManager.instance.ToggleFightingBoss(true);
            PlayerStateManager.instance.inBossFight = true;
            PlayerStateManager.instance.resources.PlayMusic_Boss1();
        }
    }

}
