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
        input.action.performed += AssignTarget;
    }

    private void OnDisable()
    {
        input.action.performed -= AssignTarget;
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

    public void AssignTarget(InputAction.CallbackContext obj)
    {
        if (isTargeting) //deactivate targeting
        {
            //swap to freelook cam
            freeLook.Priority = 10;
            targetCam.Priority = 1;

            if (currentTarget != null)
            {
                targetGroup.RemoveMember(targetPoint);
            }

            isTargeting = false;
            currentTarget = null;   
            targetPoint = null;

            return;
        }

        else //if not targeting, perform raycast to find a target
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (UnityEngine.Physics.SphereCast(mainCamera.transform.position, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, targetableLayers))
            {

                currentTarget = hit.transform;
                targetPoint = hit.collider.GetComponent<Targetable>().targetPoint;
                isTargeting = true;

                //swap to target cam, setting the current target as the targeted object in the target group
                targetGroup.AddMember(targetPoint, 1, 0);
                freeLook.Priority = 1;
                targetCam.Priority = 10;

            }

            else
            {
                if (ClosestTarget())
                {
                    currentTarget = ClosestTarget().transform;
                    targetPoint = currentTarget.GetComponent<Targetable>().targetPoint;
                    isTargeting = true;
                }
            }
        }
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
