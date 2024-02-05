using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine.InputSystem;

public class TargetLock : MonoBehaviour
{ 
    [Header("Objects")]
    [Space]
    [SerializeField] private Camera mainCamera;            // your main camera object.
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook; //cinemachine free lock camera object.
    [Space]
    [Header("UI")]
    [SerializeField] private Image aimIcon;  // ui image of aim icon u can leave it null.
    [Space]
    [Header("Settings")]
    [Space]
    [SerializeField] private string enemyTag; // the enemies tag.
    [SerializeField] private InputActionReference input;
    [SerializeField] private Vector2 targetLockOffset;
    [SerializeField] private float minDistance; // minimum distance to stop rotation if you get close to target
    [SerializeField] private float maxDistance;

    public LayerMask enemyLayer;
    
    public bool isTargeting;

    public float sphereCastRadius;
    
    private float maxAngle;
    public Transform currentTarget;
    private float mouseX;
    private float mouseY;

    private void OnEnable()
    {
        input.action.performed += AssignTarget;
    }

    private void OnDisable()
    {
        input.action.performed -= AssignTarget;
    }

    void Start()
    {
        maxAngle = 90f; // always 90 to target enemies in front of camera.
        cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
        cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
    }

    void Update()
    {
        if (!isTargeting)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        else
        {
            NewInputTarget(currentTarget);
        }

        if (aimIcon) 
            aimIcon.gameObject.SetActive(isTargeting);

        cinemachineFreeLook.m_XAxis.m_InputAxisValue = mouseX;
        cinemachineFreeLook.m_YAxis.m_InputAxisValue = mouseY;

        
    }

    private void AssignTarget(InputAction.CallbackContext obj)
    {
        if (isTargeting)
        {
            isTargeting = false;
            currentTarget = null;
            return;
        }

        //if (ClosestTarget())
        //{
        //    currentTarget = ClosestTarget().transform;
        //    isTargeting = true;
        //}

        else
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (UnityEngine.Physics.SphereCast(mainCamera.transform.position, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, enemyLayer))
            {
                if (hit.transform.CompareTag(enemyTag))
                {
                    currentTarget = hit.transform;
                    isTargeting = true;
                    Debug.Log("bruh");
                }
            }
            else
            {
                if (ClosestTarget())
                {
                    currentTarget = ClosestTarget().transform;
                    isTargeting = true;
                }
            }

            DebugExtensions.DebugSphereCast(mainCamera.transform.position, mainCamera.transform.forward, maxDistance, Color.red, sphereCastRadius, .2f, CastDrawType.Complete, PreviewCondition.Both, true);
        }
    }

    private void NewInputTarget(Transform target) // sets new input value.
    {
        if (!currentTarget) return;

        Vector3 viewPos = mainCamera.WorldToViewportPoint(target.position);
        
        if(aimIcon)
            aimIcon.transform.position = mainCamera.WorldToScreenPoint(target.position);

        if ((target.position - transform.position).magnitude < minDistance) return;
        mouseX = (viewPos.x - 0.5f + targetLockOffset.x) * 3f;              // you can change the [ 3f ] value to make it faster or  slower
        mouseY = (viewPos.y - 0.5f + targetLockOffset.y) * 3f;              // don't use delta time here.
    }


    private GameObject ClosestTarget() // this is modified func from unity Docs ( Gets Closest Object with Tag ). 
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
