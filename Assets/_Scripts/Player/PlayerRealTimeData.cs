using System.Collections.Generic;
using System.IO;
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

    [Header("Player Data")]
    public int PlayerIndex;
    public bool isNewGame = true;

    [Header("Spawn Settings (Default nhập từ Inspector)")]
    [SerializeField] private Vector3 defaultSpawnPos = Vector3.zero;
    [SerializeField] private Vector3 defaultSpawnEuler = Vector3.zero;

    [Header("Runtime Spawn (cập nhật khi checkpoint)")]
    public Vector3 spawnPos = Vector3.zero;
    public Quaternion spawnRot = Quaternion.identity;

    [Header("Quest Tracking")]
    public List<TaskID> completedMainTasks = new List<TaskID>();

    public TaskID lastCompletedMainTask;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (isNewGame || spawnPos == Vector3.zero)
            {
                spawnPos = defaultSpawnPos;
                spawnRot = Quaternion.Euler(defaultSpawnEuler);
            }

            UpdateLastCompleted();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector3 pos, Quaternion rot)
    {
        spawnPos = pos;
        spawnRot = rot;
    }

    public void AddCompletedMainTask(TaskID task)
    {
        if (!completedMainTasks.Contains(task))
        {
            completedMainTasks.Add(task);
            UpdateLastCompleted();
        }
    }

    private void UpdateLastCompleted()
    {
        if (completedMainTasks.Count > 0)
        {
            lastCompletedMainTask = completedMainTasks[completedMainTasks.Count - 1];
        }
        else
        {
            lastCompletedMainTask = TaskID.None;
        }
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "PlayerRealTimeData.json");
    }

    [System.Serializable]
    private class SaveWrapper
    {
        public SkillTreeState currentSkillTreeState;
        public List<TaskID> completedMainTasks;
        public TaskID lastCompletedMainTask;

        public float walkSpeed, runSpeed, sprintSpeed, crouchSpeed;
        public float maxStamina, staminaRecovery, sprintStamina, jumpStamina, rollStamina;
        public float jumpHeight, jumpTimer, jumpStandingDelay, airSpeed, airSmooth;
        public float fallMinHeight, fallDamage;
        public float rollSpeed, rollRotationSpeed, timeToRollAgain;
        public float freeMovementAnimatorSpeed, ReloadSpeed;
        public float PlayerMaxHealth, PlayerMaxAmour, HealthRecovery, HealthRecoveryPerTime;
        public float PlayerDamageMultiplierLonggun, PlayerDamageMultiplierShortgun;

        public Vector3 spawnPos;
        public Quaternion spawnRot;
        public int PlayerIndex;
        public bool isNewGame;
    }

    public void SaveToJson()
    {
        SaveWrapper wrapper = new SaveWrapper
        {
            currentSkillTreeState = currentSkillTreeState,
            completedMainTasks = completedMainTasks,
            lastCompletedMainTask = lastCompletedMainTask,

            walkSpeed = walkSpeed,
            runSpeed = runSpeed,
            sprintSpeed = sprintSpeed,
            crouchSpeed = crouchSpeed,

            maxStamina = maxStamina,
            staminaRecovery = staminaRecovery,
            sprintStamina = sprintStamina,
            jumpStamina = jumpStamina,
            rollStamina = rollStamina,

            jumpHeight = jumpHeight,
            jumpTimer = jumpTimer,
            jumpStandingDelay = jumpStandingDelay,
            airSpeed = airSpeed,
            airSmooth = airSmooth,

            fallMinHeight = fallMinHeight,
            fallDamage = fallDamage,

            rollSpeed = rollSpeed,
            rollRotationSpeed = rollRotationSpeed,
            timeToRollAgain = timeToRollAgain,

            freeMovementAnimatorSpeed = freeMovementAnimatorSpeed,
            ReloadSpeed = ReloadSpeed,

            PlayerMaxHealth = PlayerMaxHealth,
            PlayerMaxAmour = PlayerMaxAmour,
            HealthRecovery = HealthRecovery,
            HealthRecoveryPerTime = HealthRecoveryPerTime,

            PlayerDamageMultiplierLonggun = PlayerDamageMultiplierLonggun,
            PlayerDamageMultiplierShortgun = PlayerDamageMultiplierShortgun,

            spawnPos = spawnPos,
            spawnRot = spawnRot,
            PlayerIndex = PlayerIndex,
            isNewGame = isNewGame
        };

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(GetSavePath(), json);
    }

    public void LoadFromJson()
    {
        string path = GetSavePath();
        if (!File.Exists(path))
        {
            Debug.LogWarning("Save file not found: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        SaveWrapper wrapper = JsonUtility.FromJson<SaveWrapper>(json);

        currentSkillTreeState = wrapper.currentSkillTreeState;
        completedMainTasks = wrapper.completedMainTasks ?? new List<TaskID>();
        lastCompletedMainTask = wrapper.lastCompletedMainTask;

        if (lastCompletedMainTask == null)
            UpdateLastCompleted();

        walkSpeed = wrapper.walkSpeed;
        runSpeed = wrapper.runSpeed;
        sprintSpeed = wrapper.sprintSpeed;
        crouchSpeed = wrapper.crouchSpeed;

        maxStamina = wrapper.maxStamina;
        staminaRecovery = wrapper.staminaRecovery;
        sprintStamina = wrapper.sprintStamina;
        jumpStamina = wrapper.jumpStamina;
        rollStamina = wrapper.rollStamina;

        jumpHeight = wrapper.jumpHeight;
        jumpTimer = wrapper.jumpTimer;
        jumpStandingDelay = wrapper.jumpStandingDelay;
        airSpeed = wrapper.airSpeed;
        airSmooth = wrapper.airSmooth;

        fallMinHeight = wrapper.fallMinHeight;
        fallDamage = wrapper.fallDamage;

        rollSpeed = wrapper.rollSpeed;
        rollRotationSpeed = wrapper.rollRotationSpeed;
        timeToRollAgain = wrapper.timeToRollAgain;

        freeMovementAnimatorSpeed = wrapper.freeMovementAnimatorSpeed;
        ReloadSpeed = wrapper.ReloadSpeed;

        PlayerMaxHealth = wrapper.PlayerMaxHealth;
        PlayerMaxAmour = wrapper.PlayerMaxAmour;
        HealthRecovery = wrapper.HealthRecovery;
        HealthRecoveryPerTime = wrapper.HealthRecoveryPerTime;

        PlayerDamageMultiplierLonggun = wrapper.PlayerDamageMultiplierLonggun;
        PlayerDamageMultiplierShortgun = wrapper.PlayerDamageMultiplierShortgun;

        spawnPos = wrapper.spawnPos;
        spawnRot = wrapper.spawnRot;
        PlayerIndex = wrapper.PlayerIndex;
        isNewGame = wrapper.isNewGame;
    }
}
