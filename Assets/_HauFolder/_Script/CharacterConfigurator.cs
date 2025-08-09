using Invector;
using Invector.vCharacterController;
using UnityEngine;

public class CharacterConfigurator : MonoBehaviour
{
    public CharacterStats stats;
    public vThirdPersonController controller;
    private Animator animator;
    public vHUDController hudController;
    public WeaponInjector weaponInjector;
    public vShooterMeleeInput MeleeInput;
    public Element CurrentElement;
    public BulletType Shottype;

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
    public float DamageRatio;
    public float PlayerCurrentHealth;
    public float PlayerMaxAmour;
    public float HealthRecovery;
    public float HealthRecoveryPerTime;
    public bool isImortal;

    [Header("Player Damage")]
    public float PlayerDamageMultiplierLonggun;
    public float PlayerDamageMultiplierShortgun;
    [Header("Player Personal firearm index")]
    public float PlayerShootingSpeed;
    public float PlayerFireRate;
    public int LongGunClipSize;
    public float GunRecoil;
    #endregion

    public int CurrentBullet;
    public int CurrentGunClipSize;
    public GunType GunType;

    // Element & ShotType
    public bool isExplosive;
    public bool isEffectMode = false;
    public int PlayerElementClass = 0;
    public int ExplosiveClass = 0;
    // Canvas skilltree
    public GameObject SkillTreePanel;
    private bool isOn = false;

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
        controller.currentShield = controller.maxShield;
        MeleeInput = GetComponent<vShooterMeleeInput>();
        Shottype = BulletType.None;
    }
    int A = 0;
    
    public void updateFireRate(float Rate)
    {
        weaponInjector.UpdatePlayerFireRate(Rate);
    }    

    #region Test Amour
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isEffectMode = !isEffectMode;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isExplosive = !isExplosive;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (PlayerElementClass <= 5)
            {
                PlayerElementClass++;
            }
            else
            {
                PlayerElementClass = 0;
            }
        }
        ApplyStats();
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    TakeDamage(100f);
        //}

        //bật tắt canvas, xây dựng tạm thời
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    isOn = !isOn;
        //    SkillTreePanel.SetActive(isOn);
        //}
    }

    public void ChangeToIce()
    {
        CurrentElement = Element.Frozen;
    }    
    public void ChangeToPoison()
    {
        CurrentElement = Element.Poison;
    }    

    public void ChangeToEletric()
    {
        CurrentElement = Element.Electric;
    }    

    public void ChangeToExplosion()
    {
        Shottype = BulletType.Explosion;
        isExplosive = true;
    }
    public void TurnOnElement()
    {
        isEffectMode = true;
    }
    public void TurnOffElement()
    {
        isEffectMode = false;
    }
    public void ChangeToNone()
    {
        Shottype = BulletType.None;
        isExplosive = false;
    }
    #endregion
    public void TakeDamage(float damageValue)
    {
        float TrueDamage = damageValue * DamageRatio;
        if (TrueDamage <= 0 || controller == null) return;

        if (controller.currentShield > 0)
        {
            if (controller.currentShield >= TrueDamage)
            {
                controller.currentShield -= TrueDamage;
                hudController.EnableDamageSprite(new vDamage(damageValue));
            }
            else
            {
                float remainingDamage = TrueDamage - controller.currentShield;
                controller.currentShield = 0;
                controller.TakeDamage(new vDamage(remainingDamage));
            }
        }
        else
        {
            controller.TakeDamage(new vDamage(TrueDamage));
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

        DamageRatio = other.DamageRatio;
        PlayerMaxAmour = other.PlayerMaxAmour;
        HealthRecovery = other.HealthRecovery;
        HealthRecoveryPerTime = other.HealthRecoveryPerTime;
        isImortal = other.isImortal;

        PlayerDamageMultiplierLonggun = other.PlayerDamageMultiplierLonggun;
        PlayerDamageMultiplierShortgun = other.PlayerDamageMultiplierShortgun;
        PlayerShootingSpeed = other.PlayerShootingSpeed;
        PlayerFireRate = other.PlayerFireRate;
        LongGunClipSize = other.LongGunClipSize;
        GunRecoil = other.GunRecoil;
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
        controller.healthRecovery = HealthRecovery;
        PlayerCurrentHealth = controller.currentHealth;

        // Animator
        if (animator != null)
        {
            animator.SetFloat("FreeMovementSpeed", freeMovementAnimatorSpeed);
            animator.SetFloat("ReloadSpeed", ReloadSpeed);
            animator.SetFloat("ShootingSpeed", PlayerShootingSpeed);
        }

    }

    #endregion

    public bool isHalfClip()
    {
        return CurrentBullet <= CurrentGunClipSize / 3;
    }
    public void AddBullet(int bullet)
    {
        weaponInjector.addBullet(bullet);
    }
}


