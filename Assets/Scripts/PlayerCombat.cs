using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("LightAttack")]
    bool canAttack = true;

    [Header("Inputs")]
    public InputActionReference lightAttack;
    public InputActionReference heavyAttack;

    [Header("Animations")]
    [SerializeField] string lightAttackAnimation;
    [SerializeField] string heavyAttackAnimation;

    // references
    [SerializeField] Animator animator;
    

    private void OnEnable()
    {
        lightAttack.action.performed += LightAttack;
        heavyAttack.action.performed += HeavyAttack;
    }

    private void OnDisable()
    {
        lightAttack.action.performed -= LightAttack;
        heavyAttack.action.performed -= HeavyAttack;
    }

    void LightAttack(InputAction.CallbackContext obj)
    {
        if (canAttack)
        {
            canAttack = false;
            animator.SetTrigger("LightAttack");
        }
        
    }

    void HeavyAttack(InputAction.CallbackContext obj)
    {
        if (canAttack)
        {
            canAttack = false;
            animator.SetTrigger("HeavyAttack");
        }
       
    }

    void ResetAttack(int attackNumber)
    {
        canAttack = true;
        Debug.Log(attackNumber);
    }
}
