using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public PlayerStateManager ps;

    public float rotationSpeed;
    public bool canRotate;

    [Header("RailCam")]
    public GameObject railCam;
    public CinemachineVirtualCamera camSettings;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;

        railCam.SetActive(false);
    }

    private void Update()
    {
        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate player object
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }

    public void ChangeRailCam(bool value)
    {
        //if ps.State == railstate
        //activate rail cam
        //set to follow the EM

        if (value == true)
        {
            railCam.SetActive(true);
            //camSettings.Follow = playerObj.GetComponent<TargetLock>().currentTarget;
            //camSettings.LookAt = playerObj.GetComponent<TargetLock>().currentTarget;
        }

        else if (value == false)
        {
            railCam.SetActive(false);
        }
    }
}
