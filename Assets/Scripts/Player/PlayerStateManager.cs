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

    // states
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerAttackState attackState = new PlayerAttackState();

    [Header("MovementInputs")]
    public InputActionReference move;
    public InputActionReference jump;

    [Header("CombatInputs")]
    public InputActionReference lightAttack;
    public InputActionReference heavyAttack;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
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

    private void Start()
    {
        currentState = moveState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.FrameUpdate(this);
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    public void SwitchState(PlayerState state)
    {
        currentState = state;
        state.EnterState(this);
    }

}
