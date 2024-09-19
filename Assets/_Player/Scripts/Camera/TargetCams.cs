using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class TargetCams : MonoBehaviour
{
    public Camera mainCamera;
    public CinemachineFreeLook freeLook;
    public CinemachineVirtualCamera targetCam;
    public CinemachineTargetGroup targetGroup;

    [SerializeField] private InputActionReference input;
    [SerializeField] private Vector2 targetLockOffset;
    [SerializeField] private float minDistance; // minimum distance to stop rotation if collision gets close to target
    public float maxDistance;
    [Space]

    public LayerMask targetableLayers;

    public bool isTargeting;

    public float sphereCastRadius;

    private float maxAngle;

    public Transform currentTarget;
    public Transform targetPoint;
    public Targetable targetable;

    [SerializeField] private string enemyTag;

    [Header("UI")]
    [SerializeField] private Image aimIcon;  // ui image of aim icon

    [Header("Timed Cancel")]
    public bool timerOn = false;
    public float timer = 0f;
    public float maxTime = 10f;

    [Header("Switch Target")]
    public List<GameObject> enemiesInRange = null;
    public List<float> enemyDistances = null;
    public bool warmup = false;
    public float warmupTimer = 1;
    public float warmupCancelTimer = .5f;
    public bool cooldown = false;
    public float cooldownTimer = 1f;
    public float mouseThreshhold = 3f;

    private void OnEnable()
    {
        input.action.performed += TargetLockInput;
    }

    private void OnDisable()
    {
        input.action.performed -= TargetLockInput;
    }

    private void Start()
    {
        InputMapManager.inputActions.Player.SwitchTarget.performed += ctx =>
        {
            OnMoveTarget(ctx);
        };
    }

    void Update()
    {
        if (aimIcon)
        {
            aimIcon.gameObject.SetActive(isTargeting);
            if (currentTarget != null)
            {
                aimIcon.transform.position = mainCamera.WorldToScreenPoint(targetPoint.position);
            }
            else if (currentTarget == null)
            {
                return;
            }
        }

        if (timerOn)
        {
            if (timer < maxTime)
            {
                timer += Time.deltaTime;
            }

            else
            {
                timerOn = false;
                timer = 0f;
                CancelLock();
            }
        }

        if (isTargeting && DistanceToTarget() > maxDistance)
        {
            CancelLock();
        }
    }

    public void TargetLockInput(InputAction.CallbackContext obj)
    {
        if (isTargeting) //deactivate targeting
        {
            CancelLock();
            return;
        }

        else //if not targeting, perform raycast to find a target
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (UnityEngine.Physics.SphereCast(mainCamera.transform.position, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, targetableLayers))
            {
                AssignTarget(hit.transform, hit.collider.GetComponent<Targetable>().targetPoint, 1, true);
            }

            else
            {
                if (ClosestTarget())
                {
                    AssignTarget(ClosestTarget().transform, currentTarget.GetComponent<Targetable>().targetPoint, 1, true);
                }
            }
        }
    }

    public void AssignTarget(Transform target, Transform point, float weight, bool changeCam)
    {
        if (currentTarget != null)
        {
            targetGroup.RemoveMember(targetPoint);
        }

        currentTarget = target;
        targetable = currentTarget.GetComponent<Targetable>();
        targetable.SetColor();
        targetPoint = point;
        isTargeting = true;
        targetGroup.AddMember(point, weight, 0);

        if (currentTarget.transform.CompareTag(enemyTag)) //show health bars when targeting
        {
            if (currentTarget.transform.gameObject.GetComponentInChildren<HealthBars>() != null)
            {
                currentTarget.transform.gameObject.GetComponentInChildren<HealthBars>().ShowBars();
            }
        }

        if (changeCam)
        {
            PlayerCameraManager.instance.SwitchNonPlayerCam(PlayerCameraManager.instance.targetCam);
        }
    }

    public void ResetTarget()
    {
        if (currentTarget != null)
        {
            targetGroup.RemoveMember(targetPoint);
            targetable.ResetColor();

            if (currentTarget.transform.CompareTag(enemyTag)) //show health bars for a short time after targeting
            {
                if (currentTarget.transform.gameObject.GetComponentInChildren<HealthBars>() != null)
                {
                    currentTarget.transform.gameObject.GetComponentInChildren<HealthBars>().TargetEnding();
                }
            }
        }

        isTargeting = false;
        currentTarget = null;
        targetPoint = null;
        targetable = null;

        return;
    }

    public GameObject ClosestTarget() // this is modified func from unity Docs (Gets Closest Object with Tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float distance = maxDistance;
        float currAngle = maxAngle;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                Vector3 viewPos = mainCamera.WorldToViewportPoint(go.transform.position);
                Vector2 newPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f);
                if (Vector3.Angle(diff.normalized, mainCamera.transform.forward) < maxAngle)
                {
                    closest = go;
                    currAngle = Vector3.Angle(diff.normalized, mainCamera.transform.forward.normalized);
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    private void OnMoveTarget(InputAction.CallbackContext context)
    {
        if (isTargeting) //ensure player is targeting first
        {
            Vector2 input = context.ReadValue<Vector2>();

            

            if (input.x < -mouseThreshhold) // Move target to the left
            {
                if (currentTarget != null && SelectLeftTarget() != null && !cooldown)
                {
                    if (!warmup && !cooldown)
                    {
                        StartCoroutine(TargetWarmup());
                    }
                    else if (warmup && !cooldown)
                    {
                        warmup = false;
                        StartCoroutine(TargetCooldown());
                        targetable.ResetColor();
                        AssignTarget(SelectLeftTarget().transform, SelectLeftTarget().GetComponent<Targetable>().targetPoint, 1, true);
                    }
                }
                else if (SelectLeftTarget() == null)
                {
                    return;
                }
            }
            else if (input.x > mouseThreshhold) // Move target to the right
            {
                if (currentTarget != null && SelectRightTarget() != null && !cooldown)
                {
                    if (!warmup && !cooldown)
                    {
                        StartCoroutine(TargetWarmup());
                    }
                    else if (warmup && !cooldown)
                    {
                        warmup = false;
                        StartCoroutine(TargetCooldown());
                        targetable.ResetColor();
                        AssignTarget(SelectRightTarget().transform, SelectRightTarget().GetComponent<Targetable>().targetPoint, 1, true);
                    }
                }
                else if (SelectRightTarget() == null)
                {
                    return;
                }
            }

            if (input.y < 0) // Down scroll moves right
            {
                if (currentTarget != null && SelectRightTarget() != null)
                {
                    targetable.ResetColor();
                    AssignTarget(SelectRightTarget().transform, SelectRightTarget().GetComponent<Targetable>().targetPoint, 1, true);
                }
                else if (SelectRightTarget() == null)
                {
                    return;
                }
            }
            else if (input.y > 0) // Up scroll moves left
            {
                SelectLeftTarget();
                if (currentTarget != null && SelectLeftTarget() != null)
                {
                    targetable.ResetColor();
                    AssignTarget(SelectLeftTarget().transform, SelectLeftTarget().GetComponent<Targetable>().targetPoint, 1, true);
                }
                else if (SelectLeftTarget() == null)
                {
                    return;
                }
            }
        }
    }

    public GameObject SelectRightTarget()
    {
        enemiesInRange = new List<GameObject>();
        enemyDistances = new List<float>();

        if (currentTarget == null)
        {
            Debug.Log("current target was null on move right");
            return null;
        }

        else
        {
            Vector3 ctViewPos = mainCamera.WorldToViewportPoint(currentTarget.transform.position);
            Vector2 ctActualPos = new Vector3(ctViewPos.x - 0.5f, ctViewPos.y - 0.5f);


            foreach (GameObject enemy in TargetManager.instance.targets)
            {
                if (enemy == currentTarget) //ensures it doesn't get stuck on the current target
                {
                    continue;
                }

                //check enemy is within lock on range
                Vector3 diff = enemy.transform.position - transform.position;
                float curDistance = diff.magnitude;
                if (curDistance < maxDistance)
                {
                    Vector3 enemyViewPos = mainCamera.WorldToViewportPoint(enemy.transform.position);
                    Vector2 enemyActualPos = new Vector3(enemyViewPos.x - 0.5f, enemyViewPos.y - 0.5f);

                    if (enemyActualPos.x > ctActualPos.x) //if enemy is on the right of the middle of the screen, add to new list  //was MiddleOfScreen().x- MiddleOfScreen().x
                    {
                        enemiesInRange.Add(enemy);
                        enemyDistances.Add(enemyActualPos.x);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            float[] distances = enemyDistances.ToArray();
            float minDistance = Mathf.Min(distances);

            int index = 0;

            if (enemyDistances != null)
            {
                for (int i = 0; i < distances.Length; i++) //Find the index number relative to the target with the smallest distance
                {
                    if (minDistance == distances[i])
                        index = i;
                }

                return enemiesInRange[index]; //returns the enemy with the shortest distance
            }
            else
            {
                return null;
            }
        }
    }

    public GameObject SelectLeftTarget()
    {
        enemiesInRange = new List<GameObject>();
        enemyDistances = new List<float>();

        if (currentTarget == null)
        {
            Debug.Log("current target was null on move left");
            return null;
        }

        else
        {
            Vector3 ctViewPos = mainCamera.WorldToViewportPoint(currentTarget.transform.position);
            Vector2 ctActualPos = new Vector3(ctViewPos.x - 0.5f, ctViewPos.y - 0.5f);


            foreach (GameObject enemy in TargetManager.instance.targets)
            {
                if (enemy == currentTarget) //ensures it doesn't get stuck on the current target
                {
                    continue;
                }

                //check enemy is within lock on range
                Vector3 diff = enemy.transform.position - transform.position;
                float curDistance = diff.magnitude;
                if (curDistance < maxDistance)
                {
                    Vector3 enemyViewPos = mainCamera.WorldToViewportPoint(enemy.transform.position);
                    Vector2 enemyActualPos = new Vector3(enemyViewPos.x - 0.5f, enemyViewPos.y - 0.5f);

                    if (enemyActualPos.x < ctActualPos.x) //if enemy is on the left of the middle of the screen, add to new list  //was MiddleOfScreen().x- MiddleOfScreen().x
                    {
                        enemiesInRange.Add(enemy);
                        enemyDistances.Add(enemyActualPos.x);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            

            if (enemiesInRange != null || enemyDistances != null)
            {
                float[] distances = enemyDistances.ToArray();
                float minDistance = Mathf.Max(distances);

                int index = 0;

                for (int i = 0; i < distances.Length; i++) //Find the index number relative to the target with the smallest distance
                {
                    if (minDistance == distances[i])
                        index = i;
                }

                if (index >= 0 && index <= distances.Length)
                {
                    return enemiesInRange[index]; //returns the enemy with the shortest distance
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    IEnumerator TargetWarmup()
    {
        yield return new WaitForSeconds(warmupTimer);

        warmup = true; //allows lock on change with mouse

        yield return new WaitForSeconds(warmupCancelTimer);

        if (!cooldown) //if not still moving after cancel timer, reset
        {
            warmup = false;
        }
    }

    IEnumerator TargetCooldown()
    {
        warmup = false;
        cooldown = true;
        yield return new WaitForSeconds(cooldownTimer);
        cooldown = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }

    public void StartTimer()
    {
        timer = 0f;
        timerOn = true;
    }

    public void CancelLock()
    {
        ResetTarget();

        //swap to freelook cam
        freeLook.Priority = 10;
        targetCam.Priority = 1;
    }

    public float DistanceToTarget()
    {
        float dist = Vector3.Distance(transform.position, currentTarget.transform.position);
        return dist;
    }
}
