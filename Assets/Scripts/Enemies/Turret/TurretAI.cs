using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;

public class TurretAI : MonoBehaviour
{
    public TurretState currentState { get; set; }

    [Header("References")]
    public TurretInfo info;
    public GameObject playerPos;
    public CombatManager manager;
    public EnemyAudioManager audioManager;
    public Rigidbody rb;

    [Header("Conditions")]
    public bool inAttackRange;
    public float damageRadius;


    public string debugState;

    [Header("Head Positioning")]
    public ActivateRailMovement arm;
    public GameObject turretHead;
    public Transform lookPos1, lookPos2;
    public bool reachedPos1 = false, reachedPos2 = false;
    public float lookSpeed = 5f;
    public float lookPauseDuration = 2f;
    public float lockToPlayerDuration = 0.8f;

    [Space]
    public bool locked;

    [Header("VFX")]
    public GameObject lineStartPos;
    public LineRenderer targetLine;
    public float playerYOffset = 0.5f;

    [Header("Targeting Line & Dot")]
    public GameObject dotPrefab;
    private GameObject dotInstance;
    public GameObject dotParent;

    [Space]
    public float flashInterval;
    public float flashIntervalLength = 0.4f;
    private float flashTimer = 0f;
    private bool isRed = true;

    [Space]
    public float smoothTime = 0.3f; // Smoothing time for the dot's position
    private Vector3 dotVelocity = Vector3.zero; // Velocity of the dot
    private Vector3 smoothedPosition; // Position of the dot after smoothing
    public float clipPlaneOffset = 6f;
    public float lineSmoothTime = 0.3f; // Smoothing time for the line's position
    private Vector3 lineVelocity = Vector3.zero; // Velocity of the line's endpoint
    public Vector3 smoothedLinePosition; // Position of the line's endpoint after smoothing

    [Header("Firing")]
    public float fireTimer = 5f;
    public GameObject projectile;
    public float projectileForce = 1;
    public GameObject chargeVFX;


    [Header("Death")]
    public Transform deathFirePoint;
    public bool dead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPos = PlayerStateManager.instance.gameObject;
        info.ai = this;

        currentState = new TurretIdle();
        currentState.EnterState(this);

        targetLine.positionCount = 2;
        dotInstance = Instantiate(dotPrefab);
        dotInstance.transform.SetParent(dotParent.transform);
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
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);

        if (locked)
        {
            UpdateLinePoints();
            targetLine.enabled = true;
            dotInstance.SetActive(true);
        }
        else
        {
            targetLine.enabled = false;
            dotInstance.SetActive(false);
            chargeVFX.SetActive(false);
        }
    }

    public bool InAttackRange()
    {
        if (!dead)
        {
            inAttackRange = Physics.CheckSphere(transform.position, info.stats.patrolRange, info.whatIsPlayer);
            return inAttackRange;
        }
        else
        {
            return false;
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

    public void UpdateIdleLookPosition() //move back and forth when idle
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

    public IEnumerator LookPause() //pauses movement between idle movements, for effect
    {
        yield return new WaitForSeconds(lookPauseDuration);
        UpdateIdleLookPosition();
    }

    public IEnumerator FireCountdown()
    {
        chargeVFX.SetActive(true);
        flashInterval = flashIntervalLength;

        yield return new WaitForSeconds(fireTimer /2); //changes some vfx after half the countdown

        flashInterval = flashIntervalLength /3;

        yield return new WaitForSeconds(fireTimer /2); //finishes countdown

        chargeVFX.SetActive(false);

        if (InAttackRange()) //prevent firing if player has moved out of range
        {
            currentState = new TurretFiring();
            currentState.EnterState(this);
        }
        else
        {
            yield break;
        }
    }

    void UpdateLinePoints()
    {
        if (!dead)
        {
            targetLine.SetPosition(0, lineStartPos.transform.position);
            Vector3 offsetPlayerPos = new Vector3(playerPos.transform.position.x, playerPos.transform.position.y + playerYOffset, playerPos.transform.position.z);

            // Calculate the world position of the intended second point
            Vector3 worldPosition = Camera.main.WorldToScreenPoint(offsetPlayerPos);
            worldPosition.z = Camera.main.nearClipPlane + clipPlaneOffset; // Set Z distance for the line

            // Smoothly update the line's endpoint position
            smoothedLinePosition = Vector3.SmoothDamp(targetLine.GetPosition(1), Camera.main.ScreenToWorldPoint(worldPosition), ref lineVelocity, lineSmoothTime);

            // Set the second point of the line renderer to the smoothed line position
            targetLine.SetPosition(1, smoothedLinePosition);

            // Smoothly update the dot position using the smoothed line position
            //smoothedPosition = Vector3.SmoothDamp(dotInstance.transform.position, Camera.main.WorldToScreenPoint(smoothedLinePosition), ref dotVelocity, smoothTime);

            // Set the dot's position to the smoothed position
            dotInstance.transform.position = Camera.main.WorldToScreenPoint(offsetPlayerPos);

            flashTimer += Time.deltaTime;
            if (flashTimer >= flashInterval)
            {
                // Toggle color
                isRed = !isRed;
                dotInstance.GetComponent<Image>().color = isRed ? Color.red : Color.white;
                flashTimer = 0f; // Reset timer
            }
        }
    }

    public void InstantiateProjectile()
    {
        GameObject projectileGo = Instantiate(projectile, lineStartPos.transform.position, Quaternion.identity);
        Vector3 direction = (playerPos.transform.position - lineStartPos.transform.position).normalized;
        projectileGo.GetComponent<Rigidbody>().velocity = direction * projectileForce;
    }

    private void OnDestroy() //Remove dot instance when the enemy is destroyed
    {
        if (dotInstance != null)
        {
            Destroy(dotInstance);
        }
    }
}
