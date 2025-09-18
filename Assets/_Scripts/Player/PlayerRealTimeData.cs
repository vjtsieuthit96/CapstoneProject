using UnityEngine;

public class PlayerRealTimeData : MonoBehaviour
{
    public static PlayerRealTimeData Instance { get; private set; }

    public SkillTreeState currentSkillTreeState = new SkillTreeState();
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

    [Header("Fall Damage")]
    public float fallMinHeight;
    public float fallDamage;

    [Header("Roll")]
    public float rollSpeed;
    public float rollRotationSpeed;
    public float timeToRollAgain;

    [Header("Animator Free Speed")]
    public float freeMovementAnimatorSpeed;
    public float ReloadSpeed;

    [Header("Player Health")]
    public float PlayerMaxHealth;
    public float PlayerMaxAmour;
    public float HealthRecovery;
    public float HealthRecoveryPerTime;

    [Header("Player Damage")]
    public float PlayerDamageMultiplierLonggun;
    public float PlayerDamageMultiplierShortgun;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
