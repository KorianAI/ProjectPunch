using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyInfo : MonoBehaviour
{
    public EnemySO stats;
    public EnemyAI ai;
    public NavMeshAgent agent;

    [Header("Attacking")]
    public bool canAttack = true;
    public Transform attackPoint;
    public LayerMask whatIsPlayer;

    [Header("Animations")]
    public Animator anim;

    public abstract void Idle();

    public abstract void Chase(Transform destination);

    public abstract void Attack(Transform target);

    public abstract void Stunned();

    public abstract void Dead();
}
