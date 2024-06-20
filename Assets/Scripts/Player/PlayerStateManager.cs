using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour, IKnockback
{
    public PlayerState currentState {  get; set; }

    [SerializeField] public CharacterController controller;
    [SerializeField] public Animator anim;
    [SerializeField] public GameObject speedlines;

    [Header("Checkpoint")]
    public Transform spawnPoint;

    // states
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerAttackBase attackState = new PlayerAttackBase();
    public PlayerAirState inAirState = new PlayerAirState();
    public PlayerRailState railState = new PlayerRailState();
    public PlayerStunnedState stunnedState = new PlayerStunnedState();

    [Header("CombatInputs")]
    public InputActionReference pull;
    public InputActionReference push;
    public bool canAttack;
    public Transform attackPoint;
    public Transform pushPoint;
    public float pushRange;
    public LayerMask enemyLayer;
    public float attackRange;
    public float attackDamage;
    public GameObject hitVFX;

    public EMRail rail;

    public Transform orientation;
    public float horizontalInput;
    public float verticalInput;
    public Vector3 moveDirection;
    public AnimationHandler animHandler;

    public DebugState debugState;
    public TargetLock lockOn;
    public Transform pullPosition;

    public static PlayerStateManager instance;
    public PlayerResources resources;
    public ThirdPersonCamera cam;

    public Transform playerObj;

    public float kbForce;
    public float hitstopAmnt;
    public float shakeAmnt;
    public float shakeTimer;
    public float fovChange;
    public Animator whipAnim;

    [Header("Cameras")]
    public CinemachineFreeLook playerCam;
    public CinemachineVirtualCamera railCam;
    public CinemachineVirtualCamera finisherCam;

    PlayerAudioManager audioManager;

    public PlayerMovement pm;
    public PlayerInputHandler ih;
    public PlayerCombat pc;
    public TargetCams tl;

    public string currentStateDebug;

    public bool attackHit;

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
        pull.action.performed += Pull;
        push.action.performed += Push;

        CameraManager.RegisterPC(playerCam);
        CameraManager.RegisterVC(railCam);
        CameraManager.RegisterVC(finisherCam);
    }

    private void OnDisable()
    {
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

        audioManager = GetComponent<PlayerAudioManager>();

        if (DOTween.IsTweening(transform))
        {
            transform.DOKill();
        }    
    }

    private void Update()
    {      
        currentState.FrameUpdate(this);
        
        //Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.green);
        ShowDebugState();
        currentStateDebug = currentState.ToSafeString();
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

    public bool IsInMoveState(PlayerState state)
    {
        Type stateType = currentState.GetType();
        return stateType == typeof(PlayerMoveState) ||
               stateType == typeof(PlayerAirState) ||
               stateType == typeof(PlayerIdleState);
    }


    public void Pull(InputAction.CallbackContext obj)
    {
        if (canAttack && lockOn.currentTarget != null && currentState != stunnedState)
        {
            var target = lockOn.currentTarget.gameObject.GetComponent<IMagnetisable>();         

            if (target != null)
            {
                canAttack = false;
                RotateToTarget();
                anim.Play("Pull");
                audioManager.Pull();
                StopCoroutine("TargetPull");
                StartCoroutine("TargetPull");
            }
        } 
    }

    public IEnumerator TargetPull()
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
       
       
        //SwitchState(moveState);
    }

    public void Push(InputAction.CallbackContext obj)
    {
        if (currentState == stunnedState) { return; }

        if (canAttack && lockOn.currentTarget != null)
        {
            RotateToTarget();

            if (lockOn.currentTarget.gameObject.GetComponent<IMagnetisable>() != null)
            {
                anim.Play("Push");
                audioManager.Push();
                StopCoroutine("PushTarget");
                StartCoroutine(PushTarget(lockOn.currentTarget.gameObject.GetComponent<IMagnetisable>()));
            }
        } // targetting something

        if (currentState == railState && canAttack)
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

            
        } // on rail

       if (canAttack) // not targetting
        {
            anim.Play("Push");
            audioManager.Push();



            Collider[] colliders = Physics.OverlapSphere(pushPoint.position, pushRange);
            foreach (Collider collider in colliders)
            {
               
                Debug.Log(collider);

                IMagnetisable target = collider.GetComponent<IMagnetisable>();
                if (target != null)
                {
                    canAttack = false;
                    StartCoroutine(PushTarget(target));
                }
            }

           
        }
    }

    public IEnumerator PushTarget(IMagnetisable target)
    {
        yield return new WaitForSeconds(0.25f);
        target.Push(this);      
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

    public void CheckForEnemies(float attackType)
    {
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider c in enemies)
        {
            if (attackType == 1) // light
            {
                c.GetComponent<IDamageable>().TakeDamage(attackDamage);
            }

            if (attackType == 2) // heavy
            {
                c.GetComponent<IDamageable>().TakeDamage(attackDamage * 1.5f);
                attackHit = true;
                HitstopManager.Instance.TriggerHitstop(hitstopAmnt, gameObject, c.gameObject);
                CinemachineShake.Instance.ShakeCamera(shakeAmnt, shakeTimer);
                CinemachineShake.Instance.ChangeFov(fovChange, shakeTimer);
                RumbleManager.instance.RumblePulse(.15f, .25f, .3f);
                transform.DOKill();
            }

            if (attackType == 3) // shotgun
            {
                c.GetComponent<IDamageable>().TakeDamage(attackDamage * 1.5f);
            }

            //c.GetComponent<IKnockback>().Knockback(1.5f, orientation);

            GameObject hitParticle = Instantiate(hitVFX, c.transform);
           

            if (c.GetComponent<EnemyHealth>() != null)
            {
                c.GetComponent<EnemyHealth>().GetStunned(.2f);
               
            }

            
        }


    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pushPoint.position, pushRange);
    }

    public void Knockback(float distance, Transform attacker, float length)
    {
        if (resources.invincible) return;

        StopAllCoroutines();
        resources.invincible = true;
        SwitchState(stunnedState);
        anim.SetBool("Stunned", true);
        anim.SetTrigger("StunnedTrigger");

        StartCoroutine(StunTimer());

        IEnumerator StunTimer()
        {
            yield return new WaitForSeconds(.2f);
            Vector3 knockbackDirection = -transform.forward;
            // Calculate the knockback destination
            Vector3 knockbackDestination = transform.position + knockbackDirection * distance;
            transform.DOMove(knockbackDestination, length);
        }

    }

    public void RecoverFromStun()
    {
        SwitchState(idleState);
        anim.SetBool("Stunned", false);
        resources.invincible = false;
    }
}
