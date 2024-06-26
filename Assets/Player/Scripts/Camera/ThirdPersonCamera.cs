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


    public CinemachineBrain brain;

    public CinemachineInputProvider[] inputProvider;
    public bool blending;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;


    }

    private void Update()
    {
        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate collision object
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero && canRotate)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        if (brain.IsBlending && !blending)
        {
            blending = true;
            foreach (CinemachineInputProvider provider in inputProvider)
            {
                provider.enabled = false;
            }
        }

        else if (!brain.IsBlending && blending)
        {
            blending= false;
            foreach (CinemachineInputProvider provider in inputProvider)
            {
                provider.enabled = true;
            }
        }
    }


}
