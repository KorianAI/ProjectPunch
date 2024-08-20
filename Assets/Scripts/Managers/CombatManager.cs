using Autodesk.Fbx;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public bool combatActive;
    public List<EnemyAI> enemies;
    public List<EnemyAI> circlingEnemies;
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

    public bool tutToPlay;
    public TutorialTrigger requiredTut;

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

        if (tutToPlay && requiredTut != null && requiredTut.startCombatAfter) //if there is a tutorial to play, wait until that is closed before starting combat
        {
            requiredTut.ActivateTut(this); //passes through this combat manager, ensuring that combat will be activated when tut is closed
        }

        else if (!tutToPlay || requiredTut == null || requiredTut.startCombatAfter) //if no tutorial to play, start the combat
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
            SelectCircleEnemies();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCombat();
        }
    }

    public void SelectCircleEnemies()
    {
        List<EnemyAI> availableEnemies = new List<EnemyAI>();
        foreach (EnemyAI e in enemies)
        {
            availableEnemies.Add(e); // adds all enemies to an array
        }

        if (availableEnemies.Count > 0) // checks if array is bigger thn 0
        {
            int loopAmnt = availableEnemies.Count;
            if (loopAmnt > 3) { loopAmnt = 3; }

            for (int i = 0; i < loopAmnt; i++) // loops three times through 
            {
                int randomIndex = Random.Range(0, availableEnemies.Count); // picks three at random and then adds to circling enemies
                circlingEnemies.Add(availableEnemies[randomIndex]);
                availableEnemies.Remove(availableEnemies[randomIndex]);
            }
        }

        foreach (EnemyAI cE in circlingEnemies) // sets all circling enemies to true
        {
            cE.circleToken = true;
        }

        StartCoroutine(RandomEnemy());
    }

    public IEnumerator RandomEnemy()
    {
        List<EnemyAI> availableEnemies = new List<EnemyAI>();
        foreach (EnemyAI enemyAI in circlingEnemies)
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
            chosenEnemy.attackToken = true;
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

}
