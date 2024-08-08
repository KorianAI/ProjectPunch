using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAttackBase : PlayerState
{
    // attack dur
    public float duration;
    protected Animator animator;
    protected bool canAttack = false;

    // attack movement
    protected bool moveForward;
    public float atkMoveDistance;
    public float atkMoveDur;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    public bool isMovingForward;

    // rotating to enemy 
    protected bool isRotating = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float rotationElapsedTime = 0f;

    public bool rangeAttack;
    public bool airAttack;

    public float yPos;

    Vector3 target;

    public override void EnterState(PlayerStateManager player)
    {

        base.EnterState(player);
        RotateTowardsTarget(_sm, .1f);
        animator = GetComponent<Animator>();
        _sm.cam.canRotate = false;
        _sm.ih.SetCanConsumeInput(false);
    }

    public override void ExitState(PlayerStateManager player)
    {
        base.ExitState(player);
    }

    public override void FrameUpdate(PlayerStateManager player)
    {
        base.FrameUpdate(player);
       
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        base.HandleBufferedInput(command);
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

    #region MoveForward
    protected void MoveForward(PlayerStateManager player, float moveDistance, float moveDuration)
    {
        if (airAttack) { return;}
        isMovingForward = true;
        _sm.attackHit = false;
        initialPosition = player.transform.position;


        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = _sm.orientation.forward * verticalInput + _sm.orientation.right * horizontalInput;
        if (inputDir == Vector3.zero)
        {
            inputDir = _sm.playerObj.transform.forward; // Default to forward if no input
        }


        targetPosition = player.transform.position + inputDir * moveDistance;

        player.StartCoroutine(MoveForwardCoroutine(player, moveDuration));
    }

    private IEnumerator MoveForwardCoroutine(PlayerStateManager player, float moveDuration)
    {
        yield return new WaitForSeconds(0f);
        player.transform.DOMove(targetPosition, atkMoveDur);

        isMovingForward = false;
    }
    #endregion

    #region RotateToTarget
    protected void RotateTowardsTarget(PlayerStateManager player, float rotationDuration)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(horizontalInput, 0f, verticalInput);

        if (player.tl.currentTarget != null)
        {
            target = player.tl.currentTarget.transform.position;
        }

        else
        {
            var t = player.pc.ClosestEnemy();
            if (t.transform == null) { return; }
            target = t.transform.position;
        }



        if (inputDir != Vector3.zero && !rangeAttack && Vector3.Distance(player.transform.position, target) > 4) return;

        isRotating = true;
        initialRotation = player.transform.rotation;
        Vector3 directionToTarget = (target - player.transform.position).normalized;
        targetRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        rotationElapsedTime = 0f;

        player.StartCoroutine(RotateTowardsTargetCoroutine(player, rotationDuration));

    }


    private IEnumerator RotateTowardsTargetCoroutine(PlayerStateManager player, float rotationDuration)
    {
        while (rotationElapsedTime < rotationDuration)
        {
            rotationElapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(rotationElapsedTime / rotationDuration);
            player.playerObj.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, progress);
            yield return null;
        }

        isRotating = false;

        if (Vector3.Distance(player.transform.position, target) > 3)
        MoveForward(_sm, atkMoveDistance, atkMoveDur);
    }
    #endregion


}
