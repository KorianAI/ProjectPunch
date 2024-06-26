using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Rendering;

public class TargetLock : MonoBehaviour
{ 
    [Header("Objects")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook; //cinemachine free lock camera object
    [Space]

    [Header("UI")]
    [SerializeField] private Image aimIcon;  // ui image of aim icon
    [Space]

    [Header("Settings")]
    [SerializeField] private string enemyTag;
    [SerializeField] private string railTag;
    [SerializeField] private string lightTag;
    [SerializeField] private string shredderTag;
    [SerializeField] private string crusherTag;
    [Space]

    [SerializeField] private InputActionReference input;
    [SerializeField] private Vector2 targetLockOffset;
    [SerializeField] private float minDistance; // minimum distance to stop rotation if collision gets close to target
    [SerializeField] private float maxDistance;
    [Space]

    public LayerMask targetableLayers;
    
    public bool isTargeting;

    public float sphereCastRadius;
    
    private float maxAngle;

    [Space]
    public Transform currentTarget;
    public GameObject lastTarget;
    public string lastTargetTag; //required to allow players to push away from object when not locked on

    private float mouseX;
    private float mouseY;

    //public RaycastHit[] yikes;

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
        maxAngle = 15f; // always 90 to target enemies in front of camera.
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
            NewInputTarget(currentTarget.GetComponentInChildren<Targetable>().targetPoint);
        }

        if (aimIcon) 
            aimIcon.gameObject.SetActive(isTargeting);

        cinemachineFreeLook.m_XAxis.m_InputAxisValue = mouseX;
        cinemachineFreeLook.m_YAxis.m_InputAxisValue = mouseY;        
    }

    public void AssignTarget(InputAction.CallbackContext obj)
    {
        if (isTargeting) //deactivate targeting
        {
            isTargeting = false;
            currentTarget = null;

            if (lastTarget != null && lastTarget.gameObject.CompareTag("Enemy") == true)
            {
                if (lastTarget.GetComponentInChildren<HealthBars>() != null)
                {
                    lastTarget.GetComponentInChildren<HealthBars>().ShowBarsTargeted();
                }
                
            }

            return;
        }

        //if (ClosestTarget())
        //{
        //    currentTarget = ClosestTarget().transform;
        //    isTargeting = true;
        //}

        else //if not targeting, perform raycast to find a target
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (UnityEngine.Physics.SphereCast(mainCamera.transform.position, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, targetableLayers))
            {

                if (hit.transform.CompareTag(enemyTag))
                {
                    lastTargetTag = enemyTag;
                    lastTarget = hit.transform.gameObject;
                    if (hit.transform.gameObject.GetComponentInChildren<HealthBars>() != null)
                    {
                        hit.transform.gameObject.GetComponentInChildren<HealthBars>().ShowBars();
                    }
                    
                }

                if (hit.transform.CompareTag(railTag))
                {
                    lastTargetTag = railTag;
                }

                if (hit.transform.CompareTag(lightTag))
                {
                    lastTargetTag = lightTag;
                }

                if (hit.transform.CompareTag(shredderTag))
                {
                    lastTargetTag = shredderTag;
                }

                if (hit.transform.CompareTag(crusherTag))
                {
                    lastTargetTag = crusherTag;
                }


                currentTarget = hit.transform;
                isTargeting = true;
            }

            else
            {
                if (ClosestTarget())
                {
                    currentTarget = ClosestTarget().transform;
                    lastTargetTag = ClosestTarget().tag;
                    isTargeting = true;
                }
            }
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

    //public GameObject ClosestTarget() // this is modified func from unity Docs (Gets Closest Object with Tag)
    //{
    //    GameObject[] gos;
    //    gos = GameObject.FindGameObjectsWithTag(enemyTag);
    //    GameObject closest = null;
    //    float distance = maxDistance;
    //    float currAngle = maxAngle;
    //    Vector3 position = transform.position;
    //    foreach (GameObject go in gos)
    //    {
    //        Vector3 diff = go.transform.position - position;
    //        float curDistance = diff.magnitude;
    //        if (curDistance < distance)
    //        {
    //            Vector3 viewPos = mainCamera.WorldToViewportPoint(go.transform.position);
    //            Vector2 newPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f);
    //            if (Vector3.Angle(diff.normalized, mainCamera.transform.forward) < maxAngle)
    //            {
    //                closest = go;
    //                currAngle = Vector3.Angle(diff.normalized, mainCamera.transform.forward.normalized);
    //                distance = curDistance;
    //            }
    //        }
    //    }
    //    return closest;
    //}

    public GameObject ClosestTarget() // this is modified func from unity Docs (Gets Closest Object with Tag)
    {
        List<GameObject> enemies = GameObject.FindGameObjectsWithTag(enemyTag).ToList();
        List<GameObject> lights = GameObject.FindGameObjectsWithTag(lightTag).ToList();
        List<GameObject> rails = GameObject.FindGameObjectsWithTag(railTag).ToList();
        List<GameObject> shredders = GameObject.FindGameObjectsWithTag(shredderTag).ToList();
        List<GameObject> crushers = GameObject.FindGameObjectsWithTag(crusherTag).ToList();

        List<GameObject> gos = enemies.Union(lights).ToList();
        gos = gos.Union(rails).ToList();
        gos = gos.Union(shredders).ToList();
        gos = gos.Union(crushers).ToList();

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
