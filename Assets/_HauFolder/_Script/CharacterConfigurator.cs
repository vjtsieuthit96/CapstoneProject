using UnityEngine;
using Invector.vCharacterController;

public class CharacterConfigurator : MonoBehaviour
{
    public CharacterStats stats;
    private vThirdPersonController controller;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<vThirdPersonController>();
        if (stats != null && controller != null)
        {
            ApplyStats(stats);
        }
    }

    public void ApplyStats(CharacterStats s)
    {
        // Movement Speed
        controller.freeSpeed.walkSpeed = s.walkSpeed;
        controller.freeSpeed.runningSpeed = s.runSpeed;
        controller.freeSpeed.sprintSpeed = s.sprintSpeed;
        controller.freeSpeed.crouchSpeed = s.crouchSpeed;

        controller.strafeSpeed.walkSpeed = s.walkSpeed;
        controller.strafeSpeed.runningSpeed = s.runSpeed;
        controller.strafeSpeed.sprintSpeed = s.sprintSpeed;
        controller.strafeSpeed.crouchSpeed = s.crouchSpeed;

        // Stamina
        controller.maxStamina = s.maxStamina;
        controller.staminaRecovery = s.staminaRecovery;
        controller.sprintStamina = s.sprintStamina;
        controller.jumpStamina = s.jumpStamina;
        controller.rollStamina = s.rollStamina;

        // Jump & Air
        controller.jumpHeight = s.jumpHeight;
        controller.jumpTimer = s.jumpTimer;
        controller.jumpStandingDelay = s.jumpStandingDelay;
        controller.airSpeed = s.airSpeed;
        controller.airSmooth = s.airSmooth;
        controller.extraGravity = s.extraGravity;
        controller.limitFallVelocity = s.limitFallVelocity;

        // Fall Damage
        controller.fallMinHeight = s.fallMinHeight;
        controller.fallMinVerticalVelocity = s.fallMinVerticalVelocity;
        controller.fallDamage = s.fallDamage;

        // Roll
        controller.rollSpeed = s.rollSpeed;
        controller.rollRotationSpeed = s.rollRotationSpeed;
        controller.rollExtraGravity = s.rollExtraGravity;
        controller.timeToRollAgain = s.timeToRollAgain;
        controller.noDamageWhileRolling = s.noDamageWhileRolling;
        controller.noActiveRagdollWhileRolling = s.noActiveRagdollWhileRolling;

        // Animator
        if (animator != null)
        {
            animator.SetFloat("FreeMovementSpeed", stats.freeMovementAnimatorSpeed);
            animator.SetFloat("ReloadSpeed", stats.ReloadSpeed);
        }

    }
}
