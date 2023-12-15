using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string currentState;

    public void ChangeAnimationState(string newState)
    {
        if (newState == currentState) return;

        animator.Play(newState);
        currentState= newState;
    }
}
