using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.VFX;

public class TurretAI : MonoBehaviour
{
    public TurretState currentState { get; set; }

    [Header("References")]
    public TurretInfo info;
    public GameObject playerPos;
    public CombatManager manager;
    public EnemyAudioManager audioManager;
    public ForceField forceField;

    [Header("Conditions")]
    public bool inAttackRange;
    public Transform sightRangePos;
    public float sightRange = 35f;

    public string debugState;

    [Header("Head Positioning")]
    public GameObject turretHead;
    public Transform lookPos1, lookPos2;
    public bool reachedPos1 = false, reachedPos2 = false;
    public float lookSpeed = 5f;
    public float lookPauseDuration = 2f;
    public float lockToPlayerDuration = 1f;

    [Space]
    public bool locked;

    [Header("VFX")]
    public GameObject lineStartPos;
    public LineRenderer targetLine;
    public float playerYOffset = 0.5f;

    [Header("Targeting Line & Dot")]
    public GameObject dotPrefab;
    public GameObject dotInstance;
    public GameObject dotParent;

    [Space]
    public float flashInterval;
    public float flashIntervalLength = 0.4f;
    private float flashTimer = 0f;
    private bool isRed = true;

    [Space]
    public float dotSmoothTime = 0.1f; // Smoothing time for the dot's position
    private Vector3 dotVelocity = Vector3.zero; // Velocity of the dot
    private Vector3 smoothedPosition; // Position of the dot after smoothing
    public float clipPlaneOffset = 6f;
    public float lineSmoothTime = 0.3f; // Smoothing time for the line's position
    private Vector3 lineVelocity = Vector3.zero; // Velocity of the line's endpoint
    public Vector3 smoothedLinePosition; // Position of the line's endpoint after smoothing

    [Header("Firing")]
    public bool fired;
    public GameObject projSpawnPoint;
    public float fireTimer = 5f;
    public GameObject projectile;
    public float projectileForce = 1;
    public GameObject chargeVFX;
    public VisualEffect fireVFX;

    [Header("Death")]
    public Transform deathFirePoint;
    public bool dead = false;

    private void Start()
    {
        playerPos = PlayerStateManager.instance.gameObject;
        info.ai = this;

        currentState = new TurretIdle();
        currentState.EnterState(this);

        targetLine.positionCount = 2;
        dotInstance = Instantiate(dotPrefab);
        dotInstance.transform.SetParent(dotParent.transform);

        forceField = GetComponentInChildren<ForceField>();
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
        if (!dead)
        {
            if (locked)
            {
                UpdateLinePoints();
                targetLine.enabled = true;
                if (dotInstance != null)
                {
                    dotInstance.SetActive(true);
                }
            }
            else
            {
                targetLine.enabled = false;
                if (dotInstance != null)
                {
                    dotInstance.SetActive(false);
                }
                chargeVFX.SetActive(false);
            }
        }
    }

    public bool InAttackRange()
    {
        if (!dead)
        {
            Transform t = null;
            if (sightRangePos) { t = sightRangePos; } else { t = transform; }
            inAttackRange = Physics.CheckSphere(t.position, sightRange, info.whatIsPlayer);
            return inAttackRange;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Transform t = null;
        if (sightRangePos) { t = sightRangePos; } else { t = transform; }
        Gizmos.DrawWireSphere(t.position, sightRange);
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
        fireVFX.Play();

        if (InAttackRange()) //prevent firing if player has moved out of range
        {
            currentState = new TurretFiring();
            currentState.EnterState(this);
        }

        if (forceField != null)
        {
            yield return new WaitForSeconds(.5f);
            forceField.gameObject.GetComponent<Collider>().enabled = true;
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
            smoothedPosition = Vector3.SmoothDamp(dotInstance.transform.position, Camera.main.WorldToScreenPoint(smoothedLinePosition), ref dotVelocity, dotSmoothTime);

            // Set the dot's position to the offset position
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
        fired = true;
        GameObject projectileGo = Instantiate(projectile, projSpawnPoint.transform.position, Quaternion.identity);
        Vector3 direction = (playerPos.transform.position - projSpawnPoint.transform.position).normalized;
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
