using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public bool playStartTut;
    public TutorialTrigger requiredStartTut;

    public bool playEndTut;
    public TutorialTrigger requiredEndTut;

    public Animation combatStartAnim;


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
        //CameraManager.SwitchNonPlayerCam(entranceDoorCam); //door cam
        entranceDoorOpen.Play();

        //yield return new WaitForSecondsRealtime(4);

        //CameraManager.SwitchNonPlayerCam(enemyCam); //enemy cam

        //yield return new WaitForSecondsRealtime(4);

        //CameraManager.SwitchPlayerCam(PlayerStateManager.instance.playerCam); //return to collision cam

        yield return new WaitForSecondsRealtime(0);

        if (playStartTut && requiredStartTut != null && requiredStartTut.startCombatAfter) //if there is a tutorial to play, wait until that is closed before starting combat
        {
            requiredStartTut.ActivateTut(this); //passes through this combat manager, ensuring that combat will be activated when tut is closed
        }

        else if (!playStartTut || requiredStartTut == null || requiredStartTut.startCombatAfter) //if no tutorial to play, start the combat
        {
            StartCombat();
        }
    }

    public void StartCombat()
    {
        combatActive = true;

        if (combatStartAnim != null)
        {
            combatStartAnim.Play();
        }

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
