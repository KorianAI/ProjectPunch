using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class TurretInfo : MonoBehaviour
{
    public EnemySO stats;
    public TurretAI ai;
    public GameObject hitVFX;

    [Header("Attacking")]
    public bool canAttack = true;
    public Transform attackPoint;
    public LayerMask whatIsPlayer;

    [Header("Animations")]
    public Animator anim;

    public abstract void Idle();

    public abstract void Chase(Transform player);

    public abstract void Attack(Transform player);

    public abstract void Stunned(Transform player);

    public abstract void Dead();
}
