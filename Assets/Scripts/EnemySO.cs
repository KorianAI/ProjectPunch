using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "EnemyStats")]
public class EnemySO : ScriptableObject
{
    public float health;
    public float range;
    public float attackSpeed;
    public float damage;
    public float moveSpeed;
}
