using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class TurretAI : MonoBehaviour
{
    public TurretState currentState { get; set; }

    [Header("References")]
    //public CombatManager manager;
    public TurretInfo info;
    public GameObject playerPos;
    //public GameObject enemyVisuals;
    //public Rigidbody rb;

    [Header("Conditions")]
    public bool aggro;
    public bool inAttackRange;
    public bool available;
    public bool inAir;
    public float enemyHeight;
    public LayerMask whatIsGround;

    public float damageRadius;

    public Vector3 debugDestination;
    NavMeshHit hit;

    public EnemyAudioManager audioManager;

    public string debugState;

    public ActivateRailMovement arm;
    public Transform lookPos1, lookPos2;
    public bool reachedPos1 = false, reachedPos2 = false;
    public GameObject turretHead;
    public float lookSpeed = 3f;
    public float fireTimer = 5f;

    private void Start()
    {
        playerPos = PlayerStateManager.instance.gameObject;
        info.ai = this;

        currentState = new TurretIdle();
        currentState.EnterState(this);
    }

    public void SwitchState(TurretState _state)
    {
        _state.EnterState(this);
        currentState = _state;
        _state.ExitState(this);

    }

    private void Update()
    {
        IsGrounded();
        currentState.FrameUpdate(this);
        debugState = currentState.ToString();

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            currentState = new TurretIdle();
            currentState.EnterState(this);
        }
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    public void IsGrounded()
    {
        RaycastHit debugHit;
        bool groundRaycast = Physics.Raycast(transform.position, Vector3.down, out debugHit, enemyHeight * 0.5f + 0.2f, whatIsGround);
        if (groundRaycast)
        {
            inAir = false;
        }

        if (!groundRaycast)
        {
            inAir = true;
        }
    }

    public bool InAttackRange()
    {
        inAttackRange = Physics.CheckSphere(transform.position, info.stats.patrolRange, info.whatIsPlayer);
        return inAttackRange;
    }

    public void CheckForPlayer() //redo for turret head movement
    {
        Collider[] enemies = Physics.OverlapSphere(info.attackPoint.position, info.stats.range, info.whatIsPlayer);
        if (enemies.Length <= 0) { return; }
        foreach (Collider c in enemies)
        {
            c.GetComponent<IDamageable>().TakeDamage(info.stats.damage);
            GameObject hitVFX = Instantiate(info.hitVFX, info.attackPoint);
            //c.GetComponent<IKnockback>().Knockback(1.5f, orientation);
            //RumbleManager.instance.RumblePulse(.25f, 1f, .25f);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(info.transform.position, info.stats.patrolRange);
    }

    public void ActivateRailMovement()
    {
        if (arm != null)
        {
            arm.Defeated();
        }
    }

    public void UpdateLookPosition()
    {
        //start by moving to one of the positions
        //when position reached, move to other position
        //repeat

        if (!reachedPos1 && reachedPos2)
        {
            reachedPos1 = true;
            reachedPos2 = false;
            turretHead.transform.DOLookAt(lookPos1.transform.position, lookSpeed).SetEase(Ease.Linear).OnComplete(StartLookPause);
        }

        else if (reachedPos1 && !reachedPos2)
        {
            reachedPos1 = false;
            reachedPos2 = true;
            turretHead.transform.DOLookAt(lookPos2.transform.position, lookSpeed).SetEase(Ease.Linear).OnComplete(StartLookPause);
        }

        else //defaults to pos 1
        {
            reachedPos1 = true;
            reachedPos2 = false;
            turretHead.transform.DOLookAt(lookPos1.transform.position, lookSpeed).SetEase(Ease.Linear).OnComplete(StartLookPause);
        }
    }

    void StartLookPause()
    {
        StartCoroutine(LookPause());
    }

    public IEnumerator LookPause()
    {
        yield return new WaitForSeconds(1);
        UpdateLookPosition();
    }

    public IEnumerator FireCountdown()
    {
        yield return new WaitForSeconds(fireTimer);

        currentState = new TurretFiring();
        currentState.EnterState(this);
    }
}
