using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Character/Stats", order = 0)]
public class CharacterStats : ScriptableObject
{
    [Header("Movement Speeds")]
    public float walkSpeed = 1f;
    public float runSpeed = 2f;
    public float sprintSpeed = 3f;
    public float crouchSpeed = 0.8f;

    [Header("Stamina")]
    public float maxStamina = 200f;
    public float staminaRecovery = 1.5f;
    public float sprintStamina = 20f;
    public float jumpStamina = 15f;
    public float rollStamina = 25f;

    [Header("Jump & Airborne")]
    public float jumpHeight = 4f;
    public float jumpTimer = 0.3f;
    public float jumpStandingDelay = 0.25f;
    public float airSpeed = 5f;
    public float airSmooth = 6f;
    public float extraGravity = -10f;
    public float limitFallVelocity = -15f;

    [Header("Fall Damage")]
    public float fallMinHeight = 6f;
    public float fallMinVerticalVelocity = -10f;
    public float fallDamage = 10f;

    [Header("Roll")]
    public float rollSpeed = 3f;
    public float rollRotationSpeed = 20f;
    public float rollExtraGravity = -10f;
    public float timeToRollAgain = 0.75f;
    public bool noDamageWhileRolling = true;
    public bool noActiveRagdollWhileRolling = true;

    [Header("Animator Free Speed")]
    public float freeMovementAnimatorSpeed = 1f;
    public float ReloadSpeed = 1f;

    [Header("Player Health")]
    public float PlayerMaxHealth;
    public float PlayerMaxAmour;
    public float PlayerCurrentHealth;
    public float DamageRatio = 1f;
    public float HealthRecovery = 1f;
    public float HealthRecoveryPerTime = 1.5f;
    public bool isImortal = false;

    [Header("Player Damage")]
    public float PlayerDamageMultiplierLonggun = 1f;
    public float PlayerDamageMultiplierShortgun = 1f;

    [Header("Player Personal firearm index")]
    public float PlayerShootingSpeed = 1f;
    public float PlayerFireRate = 1f;
    public int LongGunClipSize = 25;
    public float GunRecoil = 1;
}
