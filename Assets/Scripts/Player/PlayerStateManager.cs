using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerState currentState {  get; set; }

    [SerializeField] public CharacterController controller;
    [SerializeField] public PlayerController movement;
    [SerializeField] public Animator anim;

    // states
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerLightAttack lightAttackState = new PlayerLightAttack();
    public PlayerHeavyAttack heavyAttackState = new PlayerHeavyAttack();
    public PlayerAirState inAirState = new PlayerAirState();
    public PlayerPullState pullState = new PlayerPullState();

    [Header("MovementInputs")]
    public InputActionReference move;
    public InputActionReference jump;

    [Header("CombatInputs")]
    public InputActionReference lightAttack;
    public InputActionReference heavyAttack;
    public InputActionReference pull;
    public InputActionReference push;
    public bool canAttack;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public float attackRange;
    public float attackDamage;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;
    public Vector3 velocity;

    public Transform orientation;
    public float horizontalInput;
    public float verticalInput;
    public Vector3 moveDirection;
    public AnimationHandler animHandler;

    [Header("Grav")]
    [SerializeField] private float gravMultiplier = 3.0f;
    private float gravity = -0.91f;   
    public float yVelocity;
    public bool grounded;

    public DebugState debugState;
    public TargetLock lockOn;
    public Transform pullPosition;

    public static PlayerStateManager instance;
    public PlayerResources resources;

    public enum DebugState
    {
        idle,
        walk,
        lightAttack,
        heavyAttack,
        inAir,
        pull,
        push
    }

    private void OnEnable()
    {
        lightAttack.action.performed += LightAttack;
        heavyAttack.action.performed += HeavyAttack;
        jump.action.performed += Jump;
        pull.action.performed += Pull;
    }

    private void OnDisable()
    {
        lightAttack.action.performed -= LightAttack;
        heavyAttack.action.performed -= HeavyAttack;
        jump.action.performed -= Jump;
        pull.action.performed -= Pull;
    }
    private void Start()
    {
        currentState = moveState;
        currentState.EnterState(this);

        readyToJump = true;
    }

    private void Update()
    {
        grounded = IsGrounded();
        ApplyGravity();

        currentState.FrameUpdate(this);

        ShowDebugState();
    }

    public void MovementInput()
    {
        Vector2 movementInput = move.action.ReadValue<Vector2>();

        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;

        velocity = moveDirection * moveSpeed + Vector3.up * yVelocity;
    }

    public void ApplyGravity()
    {
        if (IsGrounded() && currentState != inAirState)
        {
            yVelocity = -1f;
        }

        else
        {
            yVelocity += gravity * gravMultiplier * Time.deltaTime;
        }
        
        
            
    }

    public bool IsGrounded()
    {
        return controller.isGrounded;
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

    #region Combat


    public void LightAttack(InputAction.CallbackContext obj)
    {
        if (currentState == idleState || currentState == moveState)
        {
            if (canAttack)
            {
                RotateToTarget();
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
                RotateToTarget();
                SwitchState(heavyAttackState);
            }
        }
    }

    public void Pull(InputAction.CallbackContext obj)
    {
        if (canAttack && lockOn.currentTarget != null)
        {
            RotateToTarget();
            SwitchState(pullState);
        }
        
    }

    public void RotateToTarget()
    {
        if (lockOn.currentTarget != null)
        {
            transform.DOLookAt(orientation.forward, .1f);
        }
    }

    void ResetAttack(int attackNumber)
    {
        canAttack = true;
        Debug.Log(attackNumber);
        SwitchState(moveState);
    }

    public void CheckForEnemies()
    {
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider c in enemies)
        {
            c.GetComponent<IDamageable>().TakeDamage(attackDamage);
            return;
        }
    }
    #endregion

    #region Jumping
    public void Jump(InputAction.CallbackContext obj)
    {
        if (readyToJump && IsGrounded())
        {
            yVelocity = jumpForce;
            animHandler.ChangeAnimationState("PlayerJumpStart");
            SwitchState(inAirState);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void LandAnim()
    {
        animHandler.ChangeAnimationState("PlayerLand");
        Invoke("Land", anim.GetCurrentAnimatorStateInfo(0).length);
    }

    public void Land()
    {
        SwitchState(idleState);
    }
    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
