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
    public int attackIndex;

    // attack movement
    protected bool moveForward;
    public float atkMoveDistance;
    public float atkMoveDur;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float elapsedTime = 0f;
    public bool isMovingForward;
    public override void EnterState(PlayerStateManager player)
    {
        base.EnterState(player);
        animator = GetComponent<Animator>();
        _sm.inputHandler.SetCanConsumeInput(false);
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
        if (command.Type == InputType.A)
        {
            _sm.pm.Jump();
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        base.PhysicsUpdate(player);
    }

    protected void MoveForward(PlayerStateManager player, float moveDistance, float moveDuration)
    {
        isMovingForward = true;
        _sm.attackHit = false;
        initialPosition = player.transform.position;


        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir =_sm.orientation.forward * verticalInput + _sm.orientation.right * horizontalInput;
        if (inputDir == Vector3.zero)
        {
            inputDir = player.playerObj.transform.forward; // Default to forward if no input
            Debug.Log("f");
        }


        targetPosition = player.transform.position + inputDir * moveDistance;
        elapsedTime = 0f;

        player.StartCoroutine(MoveForwardCoroutine(player, moveDuration));
    }

    private IEnumerator MoveForwardCoroutine(PlayerStateManager player, float moveDuration)
    {
        while (elapsedTime < moveDuration && !_sm.attackHit)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / moveDuration);
            player.transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
            yield return null;
        }

        isMovingForward = false;
    }

}
