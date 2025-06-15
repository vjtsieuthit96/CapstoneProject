using UnityEngine;
using Invector.vCharacterController;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Invector;
using Invector.vShooter;
using System.Collections.Generic;

public class CharacterConfigurator : MonoBehaviour
{
    public CharacterStats stats;
    private vThirdPersonController controller;
    private Animator animator;
    public vHUDController hudController;
    public bool isExplosive = false;
    public bool isPhysicsDamage = true;
    public bool isIceEffect = false;
    public bool isElectricEffect = false;
    public bool isPoisonEffect = false;
    public int EffectMode = 0;

    #region Dữ Liệu Clone Từ Character Stats ra dữ liệu RunTime
    [Header("Movement Speeds")]
    public float walkSpeed;
    public float runSpeed;
    public float sprintSpeed;
    public float crouchSpeed;

    [Header("Stamina")]
    public float maxStamina;
    public float staminaRecovery;
    public float sprintStamina;
    public float jumpStamina;
    public float rollStamina;

    [Header("Jump & Airborne")]
    public float jumpHeight;
    public float jumpTimer;
    public float jumpStandingDelay;
    public float airSpeed;
    public float airSmooth;
    public float extraGravity;
    public float limitFallVelocity;

    [Header("Fall Damage")]
    public float fallMinHeight;
    public float fallMinVerticalVelocity;
    public float fallDamage;

    [Header("Roll")]
    public float rollSpeed;
    public float rollRotationSpeed;
    public float rollExtraGravity;
    public float timeToRollAgain;
    public bool noDamageWhileRolling;
    public bool noActiveRagdollWhileRolling;

    [Header("Animator Free Speed")]
    public float freeMovementAnimatorSpeed;
    public float ReloadSpeed;

    [Header("Player Health")]
    public float PlayerMaxHealth;
    public float PlayerMaxAmour;
    public float HealthRecovery;
    public float HealthRecoveryPerTime;
    public bool isImortal;

    [Header("Player Damage")]
    public float PlayerDamageMultiplierLonggun;
    public float PlayerDamageMultiplierShortgun;
    public float PlayerShootingSpeed;

#endregion
    private float CurrentHealth => controller != null ? controller.currentHealth : 0;
    public float _currentAmour;
    public float CurrentAmour
    {
        get => _currentAmour;
        set => _currentAmour = Mathf.Clamp(value, 0, controller != null ? controller.currentShield : 0);
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<vThirdPersonController>();
        if (stats != null && controller != null)
        {
            CopyFrom(stats);
        }
        isPhysicsDamage = true;
    }
    #region Test Amour
    private void Update()
    {
        ApplyStats();
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
          isExplosive = !isExplosive;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EffectMode = (EffectMode + 1) % 4;

            isPhysicsDamage = EffectMode == 0;
            isIceEffect = EffectMode == 1;
            isElectricEffect = EffectMode == 2;
            isPoisonEffect = EffectMode == 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(15f);
        }
    }
    #endregion
    public void TakeDamage(float damageValue)
    {
        if (damageValue <= 0 || controller == null) return;

        if (controller.currentShield > 0)
        {
            if (controller.currentShield >= damageValue)
            {
                controller.currentShield -= damageValue;
                hudController.EnableDamageSprite(new vDamage(damageValue));
                Debug.Log("Amour hiện tại là: " + controller.currentShield);
            }
            else
            {
                float remainingDamage = damageValue - controller.currentShield;
                controller.currentShield = 0;
                controller.TakeDamage(new vDamage(remainingDamage));
            }
        }
        else
        {
            controller.TakeDamage(new vDamage(damageValue));
        }
    }
    #region Clone Dữ Liệu ra
    public void CopyFrom(CharacterStats other)
    {
        walkSpeed = other.walkSpeed;
        runSpeed = other.runSpeed;
        sprintSpeed = other.sprintSpeed;
        crouchSpeed = other.crouchSpeed;

        maxStamina = other.maxStamina;
        staminaRecovery = other.staminaRecovery;
        sprintStamina = other.sprintStamina;
        jumpStamina = other.jumpStamina;
        rollStamina = other.rollStamina;

        jumpHeight = other.jumpHeight;
        jumpTimer = other.jumpTimer;
        jumpStandingDelay = other.jumpStandingDelay;
        airSpeed = other.airSpeed;
        airSmooth = other.airSmooth;
        extraGravity = other.extraGravity;
        limitFallVelocity = other.limitFallVelocity;

        fallMinHeight = other.fallMinHeight;
        fallMinVerticalVelocity = other.fallMinVerticalVelocity;
        fallDamage = other.fallDamage;

        rollSpeed = other.rollSpeed;
        rollRotationSpeed = other.rollRotationSpeed;
        rollExtraGravity = other.rollExtraGravity;
        timeToRollAgain = other.timeToRollAgain;
        noDamageWhileRolling = other.noDamageWhileRolling;
        noActiveRagdollWhileRolling = other.noActiveRagdollWhileRolling;

        freeMovementAnimatorSpeed = other.freeMovementAnimatorSpeed;
        ReloadSpeed = other.ReloadSpeed;

        PlayerMaxHealth = other.PlayerMaxHealth;
        PlayerMaxAmour = other.PlayerMaxAmour;
        HealthRecovery = other.HealthRecovery;
        HealthRecoveryPerTime = other.HealthRecoveryPerTime;
        isImortal = other.isImortal;

        PlayerDamageMultiplierLonggun = other.PlayerDamageMultiplierLonggun;
        PlayerDamageMultiplierShortgun = other.PlayerDamageMultiplierShortgun;
        PlayerShootingSpeed = other.PlayerShootingSpeed;
    }
    #endregion
    #region Apply dữ liệu realtime
    public void ApplyStats()
    {
        // Movement Speed
        controller.freeSpeed.walkSpeed = walkSpeed;
        controller.freeSpeed.runningSpeed = runSpeed;
        controller.freeSpeed.sprintSpeed = sprintSpeed;
        controller.freeSpeed.crouchSpeed = crouchSpeed;

        controller.strafeSpeed.walkSpeed = walkSpeed;
        controller.strafeSpeed.runningSpeed = runSpeed;
        controller.strafeSpeed.sprintSpeed = sprintSpeed;
        controller.strafeSpeed.crouchSpeed = crouchSpeed;

        // Stamina
        controller.maxStamina = maxStamina;
        controller.staminaRecovery = staminaRecovery;
        controller.sprintStamina = sprintStamina;
        controller.jumpStamina = jumpStamina;
        controller.rollStamina = rollStamina;

        // Jump & Air
        controller.jumpHeight = jumpHeight;
        controller.jumpTimer = jumpTimer;
        controller.jumpStandingDelay = jumpStandingDelay;
        controller.airSpeed = airSpeed;
        controller.airSmooth = airSmooth;
        controller.extraGravity = extraGravity;
        controller.limitFallVelocity = limitFallVelocity;

        // Fall Damage
        controller.fallMinHeight = fallMinHeight;
        controller.fallMinVerticalVelocity = fallMinVerticalVelocity;
        controller.fallDamage = fallDamage;

        // Roll
        controller.rollSpeed = rollSpeed;
        controller.rollRotationSpeed = rollRotationSpeed;
        controller.rollExtraGravity = rollExtraGravity;
        controller.timeToRollAgain = timeToRollAgain;
        controller.noDamageWhileRolling = noDamageWhileRolling;
        controller.noActiveRagdollWhileRolling = noActiveRagdollWhileRolling;


        //Player Max Health
        controller.maxHealth = PlayerMaxHealth;
        controller.maxShield = PlayerMaxAmour;
        controller.currentShield = controller.maxShield;

        // Animator
        if (animator != null)
        {
            animator.SetFloat("FreeMovementSpeed", freeMovementAnimatorSpeed);
            animator.SetFloat("ReloadSpeed", ReloadSpeed);
            animator.SetFloat("ShootingSpeed", PlayerShootingSpeed);
        }

    }
    #endregion
}
