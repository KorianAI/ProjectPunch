using Cinemachine;
using DG.Tweening;
using Dreamteck.Splines;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerStateManager : MonoBehaviour, IKnockback
{
    public PlayerState currentState {  get; set; }

    [SerializeField] public CharacterController controller;
    [SerializeField] public Animator anim;
    [SerializeField] public GameObject speedlines;
    [SerializeField] public SplineFollower splineFollower;

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
    public PlayerMagnets magnets;

    public string currentStateDebug;

    public bool attackHit;
    public bool pulling;
    public bool pushing;
    public bool bouncing;

    [Header("Tutorials")]
    TutorialManager tutManager;
    public bool tutIsActive;
    public Animation ltPressAnim;

    [Header("VFX")]
    public VisualEffect speedboost;
    public VisualEffect electricityEffect;
    public ParticleSystem parryParticles;

    [Header("Platforming")]
    public float moveToRailSpeed = 0.5f;
    public float nextRailLockDelay = .5f;
    public bool inBounceCollider;
    public Bouncepad currentPad;

    [Header("Music")]
    public bool inCombat;
    public bool inBossFight;

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
        tutManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

        if (DOTween.IsTweening(transform))
        {
            transform.DOKill();
        }    
    }

    private void Update()
    {      
        currentState.FrameUpdate(this);
        currentStateDebug = currentState.ToSafeString();

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            SwitchState(new PlayerIdleState());
            anim.Play("PlayerIdle");
        }
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

    public void CombatEnd()
    {
        resources.ActivateScrapShift(false);
        resources.shift.ActivateOverdrive(false);
        resources.ChangeGauntlets(4);
    }


    public void Pull(InputAction.CallbackContext obj)
    {

    }

 

    public void Push(InputAction.CallbackContext obj)
    {

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
        SwitchState(new PlayerStunnedState());
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
        SwitchState(new PlayerIdleState());
        cam.canRotate = true;
        anim.SetBool("Stunned", false);
        resources.invincible = false;
    }

    public void ResetSplineFollower()
    {
        if (bouncing)
        {
            bouncing = false;
        }

        splineFollower.spline = null;
        splineFollower.enabled = false;
    }

    public void HideTutorials()
    {
        tutManager.HideTutorial();
    }
}
