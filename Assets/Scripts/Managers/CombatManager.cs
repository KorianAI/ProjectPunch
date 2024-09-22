//using Autodesk.Fbx;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Enemies")]
    public bool combatActive;
    public List<EnemyAI> enemies;
    public List<TurretAI> turrets;
    public List<EnemyAI> circlingEnemies;
    public int enemiesAlive;

    public Transform player;
    public float radiusAroundTarget;

    public EnemyAI chosenEnemy;


    public CinemachineVirtualCamera finisherCam;

    [Header("Doors")]
    public Animation entranceDoorOpen;
    public CinemachineVirtualCamera entranceDoorCam;
    bool activatedArea = false;

    public CinemachineVirtualCamera enemyCam;

    public Animation exitDoorOpen;
    public CinemachineVirtualCamera exitDoorCam;

    [Header("Tutorials")]
    public bool playStartTut;
    public TutorialTrigger requiredStartTut;

    public bool playEndTut;
    public TutorialTrigger requiredEndTut;

    [Header("Combat Start")]
    public bool playCombatStartAnim = true;
    public Animation combatStartAnim;


    private void OnTriggerExit(Collider other)
    {       
        if (other.gameObject.CompareTag("Player") == true && !activatedArea)
        {
            activatedArea = true;
            StartCoroutine(DoorShut());

            if (/*enemies != null || */AliveMuzzlerCount() > 0)
            {
                other.GetComponent<TargetCams>().AssignTarget(enemies[0].transform, enemies[0].GetComponent<Targetable>().targetPoint, 1, true);
            }
            else if (turrets != null)
            {
                Debug.Log("no enemies, looking for turret");
                ForceField ff = turrets[0].GetComponentInChildren<ForceField>(); //find the forcefield of the turret to lock on to
                other.GetComponent<TargetCams>().AssignTarget(ff.transform, ff.GetComponent<Targetable>().targetPoint, 1, true);
            }
        }
    }

    public IEnumerator DoorShut()
    {
        //CameraManager.SwitchNonPlayerCam(entranceDoorCam); //door cam
        //entranceDoorOpen.Play();

        //yield return new WaitForSecondsRealtime(4);

        //CameraManager.SwitchNonPlayerCam(enemyCam); //enemy cam

        //yield return new WaitForSecondsRealtime(4);

        //CameraManager.SwitchPlayerCam(PlayerStateManager.instance.playerCam); //return to collision cam

        yield return new WaitForSecondsRealtime(0);

        if (playStartTut && requiredStartTut != null && requiredStartTut.startCombatAfter && !GameSettings.instance.skipTutorials) //if there is a tutorial to play, wait until that is closed before starting combat
        {
            requiredStartTut.ActivateTut(this); //passes through this combat manager, ensuring that combat will be activated when tut is closed
        }

        else if (!playStartTut || requiredStartTut == null || requiredStartTut.startCombatAfter || GameSettings.instance.skipTutorials) //if no tutorial to play, start the combat
        {
            StartCombat();
        }
    }

    public void StartCombat()
    {
        combatActive = true;
        MusicOverview.instance.Music_Combat1();
        PlayerStateManager.instance.resources.ChangeGauntlets(1);

        if (combatStartAnim != null && playCombatStartAnim)
        {
            combatStartAnim.Play();
        }

        foreach (EnemyAI e in enemies)
        {
            e.aggro = true;
            e.manager = this;
        }

        StartMuzzlerAI();
    }

    public void StartMuzzlerAI()
    {
        if (AliveMuzzlerCount() > 0)
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

        if (combatActive)
        {
            if (AliveEnemyCount() <= 0)
            {
                EndCombat();
            }
        }
    }

    public void SelectCircleEnemies()
    {
        List<EnemyAI> availableEnemies = new List<EnemyAI>();
        foreach (EnemyAI e in enemies)
        {
            if (!e.circleToken)
            {
                availableEnemies.Add(e); // adds all enemies to an array
            }
        }

        if (availableEnemies.Count > 0) // checks if array is bigger thn 0
        {
            int loopAmnt = 3 - circlingEnemies.Count;
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
            chosenEnemy.attackToken = true;
        }

        else
        {
            yield return new WaitForSeconds(1); StartCoroutine(RandomEnemy());
        }

    }

    public int AliveMuzzlerCount()
    {
        int muzzlerCount = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isActiveAndEnabled)
                muzzlerCount++;
        }
        return muzzlerCount;
    }

    public int AliveEnemyCount()
    {
        int muzzlerCount = 0;
        int turretCount = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isActiveAndEnabled)
                muzzlerCount++;
        }
        for (int i = 0; i < turrets.Count; i++)
        {
            TurretHealth th = turrets[i].GetComponent<TurretHealth>();
            if (th.currentHealth > 0 || th.hasArmour)
                turretCount++;
        }
        //Debug.Log("muzzlerCount: " + muzzlerCount);
        //Debug.Log("turretCount: " + turretCount);

        enemiesAlive = muzzlerCount + turretCount;
        return enemiesAlive;
    }

    public void EndCombat()
    {
        Debug.Log("combat ended");
        combatActive = false;
        MusicOverview.instance.Music_Explore();
        PlayerStateManager.instance.CombatEnd();
        exitDoorOpen.Play();

        if (playEndTut && requiredEndTut != null && !GameSettings.instance.skipTutorials)
        {
            requiredEndTut.ActivateTut(this);
        }
    }
}
