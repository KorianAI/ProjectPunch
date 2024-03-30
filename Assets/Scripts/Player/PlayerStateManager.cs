using Cinemachine;
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
    [SerializeField] public Animator anim;

    // states
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerAttack attackState = new PlayerAttack();
    public PlayerAirState inAirState = new PlayerAirState();
    public PlayerRailState railState = new PlayerRailState();

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
    public int comboCounter;
    public List<AnimatorOverrideController> lightCombo;
    public List<AnimatorOverrideController> heavyCombo;
    Coroutine lastRoutine;

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
    public EMRail rail;

    public Transform orientation;
    public float horizontalInput;
    public float verticalInput;
    public Vector3 moveDirection;
    public AnimationHandler animHandler;

    [Header("Grav")]
    public bool applyGrav = true;
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

    public Animator whipAnim;

    [Header("Cameras")]
    public CinemachineFreeLook playerCam;
    public CinemachineVirtualCamera railCam;
    public CinemachineVirtualCamera finisherCam;

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
        push,
        rail
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        lightAttack.action.performed += LightAttack;
        heavyAttack.action.performed += HeavyAttack;
        jump.action.performed += Jump;
        pull.action.performed += Pull;
        push.action.performed += Push;

        CameraManager.RegisterPC(playerCam);
        CameraManager.RegisterVC(railCam);
        CameraManager.RegisterVC(finisherCam);
    }

    private void OnDisable()
    {
        lightAttack.action.performed -= LightAttack;
        heavyAttack.action.performed -= HeavyAttack;
        jump.action.performed -= Jump;
        pull.action.performed -= Pull;
        push.action.performed -= Push;

        CameraManager.UnRegisterPC(playerCam);
        CameraManager.UnRegisterVC(railCam);
        CameraManager.UnRegisterVC(finisherCam);
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

        currentState.FrameUpdate(this);
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.green);
        ShowDebugState();

        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(Finisher());
        }
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
            if (currentState == inAirState)
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

        else if (currentState == attackState)
        {
            debugState = DebugState.lightAttack;
        }

        else if (currentState == inAirState)
        {
            debugState = DebugState.inAir;
        }

        else if (currentState == railState)
        {
            debugState = DebugState.rail;
        }

    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    public void SwitchState(PlayerState state)
    {
        //Debug.Log("Came from: " + state + " " + Time.time);
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
        //Debug.Log("Entered: " + state + " " + Time.time);
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
            if (Time.time - lastClickedTime >= .4f)
            {
                canAttack = false;
                SwitchState(attackState);
                if (lastRoutine != null) { StopCoroutine(lastRoutine); }
                
                Debug.Log("Attacked");

                if (light)
                {
                    if (resources.scrapStyle)
                    {
                        if (comboCounter == 0)
                        {
                            resources.lightStyle.Attack1(1, 1);
                        }

                        else if (comboCounter == 1)
                        {
                            resources.lightStyle.Attack2(1, 1);
                        }

                        else
                        {
                            resources.lightStyle.Attack3(1, 1);
                        }
                    }

                    else
                    {
                        anim.runtimeAnimatorController = lightCombo[comboCounter];
                        Debug.Log("LIGHT ATTACK: " + comboCounter);
                    }               
                }

                else if (!light)
                {
                    if (resources.scrapStyle)
                    {
                        if (comboCounter == 0)
                        {
                            resources.heavyStyle.Attack1(1, 1);
                        }

                        else if (comboCounter == 1)
                        {
                            resources.heavyStyle.Attack2(1, 1);
                        }

                        else
                        {
                            resources.heavyStyle.Attack3(1, 1);
                        }
                    }

                    else
                    {
                        anim.runtimeAnimatorController = heavyCombo[comboCounter];
                        Debug.Log("HEAVY ATTACK: " + comboCounter);
                    }
                    
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

    public void ExitAttack()
    {
        canAttack = true;
        EndCombo();
    }

    void EndCombo()
    {
        SwitchState(idleState);
       lastRoutine =  StartCoroutine(ResetCombo());     
    }

    IEnumerator ResetCombo()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("dude");
        comboCounter = 0;
        lastComboEnd = Time.time;
        canAttack = true;
    }

    public void Pull(InputAction.CallbackContext obj)
    {
        if (canAttack && lockOn.currentTarget != null)
        {
            var target = lockOn.currentTarget.gameObject.GetComponent<IMagnetisable>();
            canAttack = false;
            RotateToTarget();

            if (target != null)
            {
                anim.Play("Pull");
                StopCoroutine("PullEffect");
                StartCoroutine("PullEffect");
            }
        } 
    }

    public IEnumerator PullEffect()
    {
        var target = lockOn.currentTarget.gameObject;
        float duration = 0;

        if (target.CompareTag("Enemy"))
        {
            DOTween.To(() => playerCam.m_Lens.FieldOfView, x => playerCam.m_Lens.FieldOfView = x, 48, .25f);
            duration = 0.25f;
        }

        else if (target.CompareTag("Rail"))
        {
            DOTween.To(() => playerCam.m_Lens.FieldOfView, x => playerCam.m_Lens.FieldOfView = x, 85, .4f);
            duration = .4f;
        }

        else // spotlight
        {
            DOTween.To(() => playerCam.m_Lens.FieldOfView, x => playerCam.m_Lens.FieldOfView = x, 85, .4f);
            duration = .4f;
        }

        
        yield return new WaitForSeconds(duration);

        
        target.GetComponent<IMagnetisable>().Pull(this);
        canAttack = true;
       
        //SwitchState(moveState);
    }

    public void Push(InputAction.CallbackContext obj)
    {
        if (canAttack && lockOn.currentTarget != null)
        {
            RotateToTarget();

            if (lockOn.currentTarget.gameObject.GetComponent<IMagnetisable>() != null)
            {
                anim.Play("Push");
                StopCoroutine("PushEffect");
                StartCoroutine("PushEffect");
            }
        }

        if (currentState == railState)
        {
            CameraManager.SwitchPlayerCam(playerCam);

            lockOn.currentTarget = null;
            lockOn.isTargeting = false;
            lockOn.lastTargetTag = null;
            rail = null;
            cam.canRotate = true;

            transform.SetParent(null);
            currentState = inAirState;

            anim.Play("PlayerInAir");
            anim.SetBool("onRail", false);

            
        }

        else if (canAttack && lockOn.currentTarget == null)
        {
            // dodge
        }
    }

    public IEnumerator PushEffect()
    {
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
            playerObj.transform.DOLookAt(t, 0f).onComplete = CanRotate;
            
        }

        else
        {
            cam.canRotate = false;
            playerObj.forward = orientation.forward;
            CanRotate();
        }
    }

    void CanRotate()
    {
        cam.canRotate = true;
    }

    public void FuckOff(float attackNo)
    {
        //whipAnim.gameObject.SetActive(true);

        if (attackNo == 1)
        {
            whipAnim.Play("WhipEffect");
        }

        else if (attackNo == 2)
        {
            whipAnim.Play("Whip2Effect");
        }

        else if (attackNo == 3)
        {
            whipAnim.Play("Whip3Effect");
        }
        
    }

    public IEnumerator Finisher()
    {
        Time.timeScale = .5f;
        CameraManager.SwitchNonPlayerCam(finisherCam);
        yield return new WaitForSecondsRealtime(3);
        CameraManager.SwitchPlayerCam(playerCam);
        Time.timeScale = 1f;
    }

    public void CheckForEnemies()
    {
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider c in enemies)
        {
            c.GetComponent<IDamageable>().TakeDamage(attackDamage);
            c.GetComponent<IKnockback>().Knockback(1.5f, orientation);
            
        }
    }
    #endregion

    #region Jumping
    public void Jump(InputAction.CallbackContext obj)
    {
        if (currentState == railState)
        {
            CameraManager.SwitchPlayerCam(playerCam);

            lockOn.currentTarget = null;
            lockOn.isTargeting = false;
            lockOn.lastTargetTag = null;
            cam.canRotate = true;

            transform.SetParent(null);
            currentState = inAirState;

            anim.Play("PlayerInAir");
            anim.SetBool("onRail", false);
        }
        
        if (currentState != moveState && currentState != idleState) return;

        if (grounded) //readyToJump check removed due to bug (issue #3)
        {
            yVelocity = jumpForce;
            anim.Play("PlayerJumpStart");
            SwitchState(inAirState);
        }       
    }

    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
