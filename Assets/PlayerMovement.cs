using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStateManager sm;
    public CharacterController controller;
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;


    public Vector3 moveDirection;
    public Vector3 velocity;


    public Transform orientation;
    public float horizontalInput;
    public float verticalInput;

    [Header("GroundCheck")]
    public LayerMask ground;
    public float playerHeight;
    public bool readyToJump;

    [Header("Grav")]
    public bool applyGrav = true;
    [SerializeField] private float gravMultiplier = 3.0f;
    private float gravity = -0.91f;
    public float yVelocity;
    public bool grounded;

    InputMaster InputActions;

    public Animator anim;

    void Awake()
    {
        InputActions = new InputMaster();
    }

    void OnEnable()
    {
        InputActions.Enable();
        //InputActions.Player.Jump.performed += ctx => { Jump(); };
    }

    void OnDisable()
    {
        InputActions.Disable();
    }

    private void Update()
    {
        anim.SetBool("isGrounded", grounded);
        IsGrounded();
        ApplyGravity();
        MovementInput();

    }

    public void MovementInput()
    {
        Vector2 movementInput = InputActions.Player.Movement.ReadValue<Vector2>();

        if (grounded)
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;
        }

        else
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;
        }


        velocity = moveDirection * moveSpeed + Vector3.up * yVelocity;
    }

    public void ApplyGravity()
    {
        if (grounded && sm.currentState != sm.inAirState)
        {
            yVelocity = -1f;
        }

        else
        {
            if (sm.currentState == sm.inAirState || sm.currentState == sm.stunnedState)
            {
                yVelocity += gravity * gravMultiplier * Time.deltaTime;
            }

            else
            {
                yVelocity = 0f;
            }

        }
    }

    public void IsGrounded()
    {
        RaycastHit debugHit;
        bool groundRaycast = Physics.Raycast(transform.position, Vector3.down, out debugHit, playerHeight * 0.5f + 0.2f, ground);
        if (groundRaycast && controller.isGrounded)
        {
            grounded = true;
        }

        if (!groundRaycast)
        {
            grounded = false;
        }
    }

    public void Jump()
    {
        Debug.Log("1");

        if (sm.currentState == sm.railState && sm.canAttack)
        {
            CameraManager.SwitchPlayerCam(sm.playerCam);

            sm.lockOn.currentTarget = null;
            sm.lockOn.isTargeting = false;
            sm.lockOn.lastTargetTag = null;
            sm.cam.canRotate = true;

            transform.SetParent(null);
            sm.currentState = sm.inAirState;

            sm.anim.Play("PlayerInAir");
            sm.anim.SetBool("onRail", false);
        }

        //if (sm.currentState != sm.moveState && sm.currentState != sm.idleState) return;

        Debug.Log("2");

        if (grounded) //readyToJump check removed due to bug (issue #3)
        {
            yVelocity = jumpForce;
            sm.anim.Play("PlayerJumpStart");
            sm.SwitchState(sm.inAirState);
            Debug.Log("3");
        }
    }
}
