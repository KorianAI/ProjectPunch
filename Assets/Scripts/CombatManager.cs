using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public bool combatActive;
    public List<EnemyAI> enemies;
    public float enemiesAlive;

    public Transform player;
    public float radiusAroundTarget;

    public EnemyAI chosenEnemy;


    public CinemachineVirtualCamera finisherCam;

    public Animation entranceDoorOpen;
    public CinemachineVirtualCamera entranceDoorCam;
    bool playedOpen = false;

    public CinemachineVirtualCamera enemyCam;

    public Animation exitDoorOpen;
    public CinemachineVirtualCamera exitDoorCam;

    private void OnTriggerExit(Collider other)
    {       
        if (other.gameObject.CompareTag("Player") == true && !playedOpen)
        {
            playedOpen = true;
            StartCoroutine(DoorShut());
        }
    }

    public IEnumerator DoorShut()
    {
        //door cam
        CameraManager.SwitchNonPlayerCam(entranceDoorCam);
        entranceDoorOpen.Play();
        yield return new WaitForSecondsRealtime(4);

        //enemy cam
        CameraManager.SwitchNonPlayerCam(enemyCam);

        yield return new WaitForSecondsRealtime(4);

        //return to collision cam
        CameraManager.SwitchPlayerCam(PlayerStateManager.instance.playerCam);

        yield return new WaitForSecondsRealtime(4);

        StartCombat();
    }

    public void StartCombat()
    {
        combatActive = true;
        foreach (EnemyAI e in enemies)
        {
            e.aggro = true;
            e.manager = this;
        }

        StartAI();
    }

    public void StartAI()
    {
        if (AliveEnemyCount() > 0)
        {
            StartCoroutine(RandomEnemy());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCombat();
        }
    }

    public IEnumerator RandomEnemy()
    {
        List<EnemyAI> availableEnemies = new List<EnemyAI>();
        foreach (EnemyAI enemyAI in enemies)
        {
            if (enemyAI.available)
            {
                availableEnemies.Add(enemyAI);
            }
        }

        if (availableEnemies.Count > 0)
        {
            int randomIndex = Random.Range(0, availableEnemies.Count);
            chosenEnemy = availableEnemies[randomIndex];
        }

        if (chosenEnemy == null) { yield return new WaitForSeconds(2);  StartCoroutine(RandomEnemy()); }

        else
        {
            chosenEnemy.permissionToAttack = true;
        }
        yield return new WaitForSeconds(Random.Range(0, .5f));
    }

    public int AliveEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isActiveAndEnabled)
                count++;
        }

        enemiesAlive = count;
        return count;
    }

    public void MakeAgentsCircleTarget(EnemyAI ai)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == ai)
            {
                float randomI = i * Random.Range(i, i * 2);
                //radiusAroundTarget = Random.Range(3, 5);
                Vector3 target = new Vector3(player.position.x + radiusAroundTarget * Mathf.Cos(2 * Mathf.PI * randomI / enemies.Count),
                player.position.y,
                player.position.z + radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * randomI / enemies.Count));

                Debug.Log(target);
                enemies[i].agent.SetDestination(target);
            }
        }
    }

}
