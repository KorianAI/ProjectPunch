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
    public PlayerAttack lightAttackState = new PlayerAttack();
    public PlayerAirState inAirState = new PlayerAirState();

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

    [Header("Combos")]
    float lastClickedTime;
    float lastComboEnd;
    int comboCounter;
    int comboCount = 3;
    public List<AnimatorOverrideController> lightCombo;
    public List<AnimatorOverrideController> heavyCombo;

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
    public ThirdPersonCamera cam;

    public Transform playerObj;

    public float kbForce;

    [Header("GroundCheck")]
    public LayerMask ground;
    public float playerHeight;

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
        push.action.performed += Push;
    }

    private void OnDisable()
    {
        lightAttack.action.performed -= LightAttack;
        heavyAttack.action.performed -= HeavyAttack;
        jump.action.performed -= Jump;
        pull.action.performed -= Pull;
        push.action.performed -= Push;
    }
    private void Start()
    {
        currentState = moveState;
        currentState.EnterState(this);

        readyToJump = true;
    }

    private void Update()
    {
        IsGrounded();
        anim.SetBool("isGrounded", grounded);
        ApplyGravity();
        ExitAttack();

        currentState.FrameUpdate(this);
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.green);
        ShowDebugState();
    }

    public void MovementInput()
    {
        Vector2 movementInput = move.action.ReadValue<Vector2>();

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
        if (grounded && currentState != inAirState)
        {
            yVelocity = -1f;
        }

        else
        {
            yVelocity += gravity * gravMultiplier * Time.deltaTime;
        }
        
        
            
    }

    public void IsGrounded()
    {
        bool groundRaycast = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);

        if (groundRaycast && controller.isGrounded)
        {
            grounded = true;
        }

        if (!groundRaycast)
        {
            grounded = false;
        }
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
        //Debug.Log("Came from: " + state);
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
        //Debug.Log("Entered: " + state);
    }

    #region Combat


    public void LightAttack(InputAction.CallbackContext obj)
    {
        if (currentState != inAirState)
        {
            if (canAttack)
            {
                //canAttack = false;
                RotateToTarget();
                Attack(true);
            }
        }
    }
    public void HeavyAttack(InputAction.CallbackContext obj)
    {
        if (currentState != inAirState)
        {
            if (canAttack)
            {
                //canAttack = false;
                RotateToTarget();
                Attack(false);
            }
        }
    }

    void Attack(bool light)
    {
        if (Time.time - lastComboEnd > 0.5f && comboCounter < lightCombo.Count)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickedTime >= .8f)
            {
                if (light)
                {
                    anim.runtimeAnimatorController = lightCombo[comboCounter];
                    Debug.Log("LIGHT ATTACK: " + comboCounter);
                }

                else if (!light)
                {
                    anim.runtimeAnimatorController = heavyCombo[comboCounter];
                    Debug.Log("HEAVY ATTACK: " + comboCounter);
                }

                anim.Play("Attack", 0, 0);
                anim.CrossFadeInFixedTime("Attack", 0.1f);
                comboCounter++;
                lastClickedTime = Time.time;

                if (comboCounter >= lightCombo.Count)
                {
                    comboCounter = 0;
                }
            }
        }
    }

    void ExitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
        {
            canAttack = true;
            Invoke("EndCombo", 1);
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
        canAttack = true;
    }

    public void Pull(InputAction.CallbackContext obj)
    {
        if (canAttack && lockOn.currentTarget != null)
        {
            canAttack = false;
            RotateToTarget();

            if (lockOn.currentTarget.gameObject.GetComponent<IMagnetisable>() != null)
            {
                anim.Play("Pull");
                StopCoroutine("PullEffect");
                StartCoroutine("PullEffect");
            }
        } 
    }

    public IEnumerator PullEffect()
    {
        //float cd = anim.GetCurrentAnimatorStateInfo(0).length / 2.5f;
        //Debug.Log(cd);
        yield return new WaitForSeconds(0.25f);

        lockOn.currentTarget.gameObject.GetComponent<IMagnetisable>().Pull(this);
        canAttack = true;
        SwitchState(moveState);
    }

    public void Push(InputAction.CallbackContext obj)
    {
        if (canAttack && lockOn.currentTarget != null)
        {
            RotateToTarget();

            if (lockOn.currentTarget.gameObject.GetComponent<IMagnetisable>() != null)
            {
                anim.Play("Push");
                StopCoroutine("PuEffect");
                StartCoroutine("PushEffect");
            }
        }

        else if (canAttack && lockOn.currentTarget == null)
        {
            // dodge
        }
    }

    public IEnumerator PushEffect()
    {
        //float cd = anim.GetCurrentAnimatorStateInfo(0).length / 2.5f;
        //Debug.Log(cd);
        yield return new WaitForSeconds(0.25f);

        lockOn.currentTarget.gameObject.GetComponent<IMagnetisable>().Push(this);
        canAttack = true;
        SwitchState(moveState);
    }

    public void RotateToTarget()
    {
        if (lockOn.currentTarget != null)
        {
            cam.canRotate = false;
            Vector3 t = new Vector3(lockOn.currentTarget.transform.position.x, playerObj.transform.position.y, lockOn.currentTarget.transform.position.z);
            playerObj.transform.DOLookAt(t, 0f);
            
        }
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
        if (currentState != moveState && currentState != idleState) return;

        if (readyToJump && grounded)
        {
            yVelocity = jumpForce;
            anim.Play("PlayerJumpStart");
            SwitchState(inAirState);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
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
