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
            RandomEnemy();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCombat();
        }
    }


    public void RandomEnemy()
    {
        int randomIndex = Random.Range(0, enemies.Count);
        chosenEnemy = enemies[randomIndex];

        chosenEnemy.permissionToAttack = true;
        MakeAgentsCircleTarget();
        Debug.Log(chosenEnemy.name);
        foreach (EnemyAI e in enemies)
        {
            if (e != chosenEnemy)
            {
                e.permissionToAttack = false;
                e.state = EnemyAI.State.Circle;
            }
        }
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

    public void MakeAgentsCircleTarget()
    {
        Debug.Log("bruh2");
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == chosenEnemy) { continue; }
            Vector3 target = new Vector3(player.position.x + radiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / enemies.Count),
               player.position.y,
               player.position.z + radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / enemies.Count));
            
            Debug.Log(target);
            enemies[i].agent.SetDestination(target);
        }
    }
}
