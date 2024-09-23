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
            PlayMusic_Boss1();
            MusicManager.instance.ToggleFightingBoss(true);

        }
    }

    public void PlayMusic_Boss1()
    {
        playMusic_Boss1.Post(gameObject);
    }
}
