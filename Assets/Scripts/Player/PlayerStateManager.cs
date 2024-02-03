using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerState currentState {  get; set; }

    [SerializeField] public Rigidbody rb;
    [SerializeField] public PlayerController movement;
    [SerializeField] public Animator anim;

    // states
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerLightAttack lightAttackState = new PlayerLightAttack();
    public PlayerHeavyAttack heavyAttackState = new PlayerHeavyAttack();
    public PlayerAirState inAirState = new PlayerAirState();

    [Header("MovementInputs")]
    public InputActionReference move;
    public InputActionReference jump;

    [Header("CombatInputs")]
    public InputActionReference lightAttack;
    public InputActionReference heavyAttack;
    public bool canAttack;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform orientation;
    public float horizontalInput;
    public float verticalInput;
    public Vector3 moveDirection;
    public AnimationHandler animHandler;
    public string idleAnim;
    public string walkAnim;
    public string attackAnim;

    public DebugState debugState;

    public enum DebugState
    {
        idle,
        walk,
        lightAttack,
        heavyAttack,
        inAir
    }

    private void OnEnable()
    {
        lightAttack.action.performed += LightAttack;
        heavyAttack.action.performed += HeavyAttack;
        jump.action.performed += Jump;
    }

    private void OnDisable()
    {
        lightAttack.action.performed -= LightAttack;
        heavyAttack.action.performed -= HeavyAttack;
        jump.action.performed -= Jump;
    }
    private void Start()
    {
        currentState = moveState;
        currentState.EnterState(this);

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        currentState.FrameUpdate(this);

        ShowDebugState();
    }

    void ShowDebugState()
    {
        if (currentState == idleState)
        {
            debugState = DebugState.idle;
        }

        else if (currentState == moveState)
        {
            debugState = DebugState.walk;
        }

        else if (currentState == lightAttackState)
        {
            debugState = DebugState.lightAttack;
        }

        else if (currentState == heavyAttackState)
        {
            debugState = DebugState.heavyAttack;
        }

        else if (currentState == inAirState)
        {
            debugState = DebugState.inAir;
        }
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    public void SwitchState(PlayerState state)
    {
        Debug.Log("Came from: " + state);
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
        Debug.Log("Entered: " + state);
    }

    public void LightAttack(InputAction.CallbackContext obj)
    {
        if (currentState == idleState || currentState == moveState)
        {
            if (canAttack)
            {
                SwitchState(lightAttackState);
            }
        }
    }
    public void HeavyAttack(InputAction.CallbackContext obj)
    {
        if (currentState == idleState || currentState == moveState)
        {
            if (canAttack)
            {
                SwitchState(heavyAttackState);
            }
        }
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        if (readyToJump && grounded)
        {
            

            SwitchState(inAirState);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void ResetAttack(int attackNumber)
    {
        canAttack = true;
        Debug.Log(attackNumber);
        SwitchState(moveState);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}
