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


    public EMRail rail;

    public Transform orientation;



    public TargetLock lockOn;
    public Transform pullPosition;

    public static PlayerStateManager instance;
    public PlayerResources resources;
    public ThirdPersonCamera cam;

    public Transform playerObj;


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
    public bool pulling;
    public bool pushing;



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
        currentStateDebug = currentState.ToSafeString();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(this);
    }

    public void SwitchState(PlayerState state)
    {
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }


    public void Pull(InputAction.CallbackContext obj)
    {
        if (canAttack && tl.currentTarget != null && currentState != stunnedState)
        {
            var target = tl.currentTarget.gameObject.GetComponent<IMagnetisable>();         

            if (target != null)
            {
                anim.Play("Pull");
                audioManager.Pull();

            }
        } 
    }

 

    public void Push(InputAction.CallbackContext obj)
    {
        if (currentState == stunnedState) { return; }

        if (canAttack && tl.currentTarget != null)
        {

            if (tl.currentTarget.gameObject.GetComponent<IMagnetisable>() != null)
            {
                anim.Play("Push");
                audioManager.Push();
                StopCoroutine("PushTarget");
                StartCoroutine(PushTarget(tl.currentTarget.gameObject.GetComponent<IMagnetisable>()));
            }
        } // targetting something

        if (currentState == railState && canAttack)
        {
            CameraManager.SwitchPlayerCam(playerCam);

            tl.currentTarget = null;
            tl.isTargeting = false;
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
