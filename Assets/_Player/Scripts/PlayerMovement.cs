using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStateManager sm;
    public CharacterController controller;
    public PlayerCombat combat;
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

    public float airDashAmount;
    public float maxAirDash;

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

    private void Start()
    {
        combat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {       
        IsGrounded();
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

    public void ApplyGravity(float type)
    {
        if (type == 1) // grounded
        {
            yVelocity = -1f;
        }

        else if (type == 2) // full grav
        {
            yVelocity += gravity * gravMultiplier * Time.deltaTime;
        }

        else if (type == 3)
        {
            yVelocity += gravity * (gravMultiplier / 3) * Time.deltaTime;
        }

        else
        {
            yVelocity = 0f;         
        }
    }

    public void IsGrounded()
    {
        RaycastHit debugHit;
        bool groundRaycast = Physics.Raycast(transform.position, Vector3.down, out debugHit, playerHeight * 0.5f + 0.2f, ground);
        if (groundRaycast && controller.isGrounded)
        {
            grounded = true;
            anim.SetBool("isGrounded", true);
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

        JumpEffect();

        sm.SwitchState(new PlayerAirState());
    }

    public void JumpEffect()
    {
        if (jumpEffectPrefab != null && jumpEffectPosition != null)
        {
            // Instantiate the VFX at the player's feet position
            VisualEffect newJumpEffect = Instantiate(jumpEffectPrefab, jumpEffectPosition.position, Quaternion.identity);
            newJumpEffect.Play();
            Destroy(newJumpEffect.gameObject, 1f);
        }
    }
}
