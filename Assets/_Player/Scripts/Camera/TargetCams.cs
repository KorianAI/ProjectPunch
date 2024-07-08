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
    [SerializeField] private float maxDistance;
    [Space]

    public LayerMask targetableLayers;

    public bool isTargeting;

    public float sphereCastRadius;

    private float maxAngle;

    public Transform currentTarget;
    public Transform targetPoint;

    [SerializeField] private string enemyTag;

    [Header("UI")]
    [SerializeField] private Image aimIcon;  // ui image of aim icon

    private void OnEnable()
    {
        input.action.performed += TargetLockInput;
    }

    private void OnDisable()
    {
        input.action.performed -= TargetLockInput;
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
        }

    }

    public void TargetLockInput(InputAction.CallbackContext obj)
    {
        if (isTargeting) //deactivate targeting
        {
            //swap to freelook cam
            freeLook.Priority = 10;
            targetCam.Priority = 1;
            ResetTarget();
            return;
        }

        else //if not targeting, perform raycast to find a target
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (UnityEngine.Physics.SphereCast(mainCamera.transform.position, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, targetableLayers))
            {
                AssignTarget(hit.transform, hit.collider.GetComponent<Targetable>().targetPoint, 1);
            }

            else
            {
                if (ClosestTarget())
                {
                    AssignTarget(ClosestTarget().transform, currentTarget.GetComponent<Targetable>().targetPoint, 1);
                }
            }
        }
    }

    public void AssignTarget(Transform target, Transform point, float weight)
    {
        currentTarget = target;
        targetPoint = point;
        isTargeting = true;
        targetGroup.AddMember(point, weight, 0);
        PlayerCameraManager.instance.SwitchNonPlayerCam(PlayerCameraManager.instance.targetCam);
    }

    public void ResetTarget()
    {
        if (currentTarget != null)
        {
            targetGroup.RemoveMember(targetPoint);
        }

        isTargeting = false;
        currentTarget = null;
        targetPoint = null;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
