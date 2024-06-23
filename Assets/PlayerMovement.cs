using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStateManager sm;
    public CharacterController controller;
    public float currentSpeed;
    public float airSpeed;

    public float runSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public bool jumpInputCD;



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

    public VisualEffect jumpEffectPrefab;
    public Transform jumpEffectPosition;

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


        velocity = moveDirection * currentSpeed + Vector3.up * yVelocity;
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
            currentSpeed = runSpeed;
        }

        if (!groundRaycast)
        {
            grounded = false;
            currentSpeed = airSpeed;
        }
    }

    public void Jump()
    {
        //if (sm.currentState == sm.railState && sm.canAttack)
        //{
        //    CameraManager.SwitchPlayerCam(sm.playerCam);

        //    sm.lockOn.currentTarget = null;
        //    sm.lockOn.isTargeting = false;
        //    sm.lockOn.lastTargetTag = null;
        //    sm.cam.canRotate = true;

        //    transform.SetParent(null);
        //    sm.currentState = sm.inAirState;

        //    sm.anim.Play("PlayerInAir");
        //    sm.anim.SetBool("onRail", false);
        //}

        //if (sm.currentState != sm.moveState && sm.currentState != sm.idleState) return;

 

        if (grounded) //readyToJump check removed due to bug (issue #3)
        {
            sm.SwitchState(new PlayerJumpStart());
            sm.anim.Play("PlayerJumpStart");         
        }
    }

    public void JumpForce()
    {
        yVelocity = jumpForce;
        jumpInputCD = true;

        if (jumpEffectPrefab != null && jumpEffectPosition != null)
        {
            // Instantiate the VFX at the player's feet position
            VisualEffect newJumpEffect = Instantiate(jumpEffectPrefab, jumpEffectPosition.position, Quaternion.identity);
            newJumpEffect.Play();
            Destroy(newJumpEffect.gameObject, 1f);
        }

        sm.SwitchState(new PlayerAirState());
    }


}
