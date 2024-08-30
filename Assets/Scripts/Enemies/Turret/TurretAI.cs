using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class TurretAI : MonoBehaviour
{
    public TurretState currentState { get; set; }

    [Header("References")]
    public TurretInfo info;
    public GameObject playerPos;

    [Header("Conditions")]
    public bool inAttackRange;
    public float damageRadius;

    public EnemyAudioManager audioManager;

    public string debugState;

    [Header("Head Positioning")]
    public ActivateRailMovement arm;
    public GameObject turretHead;
    public Transform lookPos1, lookPos2;
    public bool reachedPos1 = false, reachedPos2 = false;
    public float lookSpeed = 5f;

    [Header("VFX")]
    public List<Vector3> linePositions;
    public GameObject lazerStartPos;
    public LineRenderer targetLine;

    [Header("Firing")]
    public float fireTimer = 5f;

    private void Start()
    {
        playerPos = PlayerStateManager.instance.gameObject;
        info.ai = this;

        currentState = new TurretIdle();
        currentState.EnterState(this);

        linePositions.Add(lazerStartPos.transform.localToWorldMatrix.GetPosition());
        targetLine.enabled = false;
    }

    public void SwitchState(TurretState _state)
    {
        _state.EnterState(this);
        currentState = _state;
        _state.ExitState(this);
    }

    private void Update()
    {
        currentState.FrameUpdate(this);
        debugState = currentState.ToString();

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            currentState = new TurretIdle();
            currentState.EnterState(this);
        }

        linePositions.RemoveAt(1);
        linePositions.Add(playerPos.transform.localToWorldMatrix.GetPosition()); //keep player pos up to date
        targetLine.SetPositions(linePositions.ToArray());
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    public bool InAttackRange()
    {
        inAttackRange = Physics.CheckSphere(transform.position, info.stats.patrolRange, info.whatIsPlayer);
        return inAttackRange;
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
        yield return new WaitForSeconds(2);
        UpdateLookPosition();
    }

    public IEnumerator FireCountdown()
    {
        //vfx
        //line between player position and lazer point pos
        targetLine.enabled = true;
        yield return new WaitForSeconds(fireTimer);

        currentState = new TurretFiring();
        currentState.EnterState(this);
    }
}
