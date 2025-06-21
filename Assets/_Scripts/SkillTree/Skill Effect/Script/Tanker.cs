using UnityEngine;

[CreateAssetMenu(fileName = "Tanker", menuName = "Scriptable Objects/DefenseHealth/Tanker")]
public class Tanker : SkillEffect
{
    [Header("Damage Ratio Percent")]
    [Range(0f, 1f)]
    public float RatioDamagePercent = 0.1f;

    [Header(" Amour Percent")]
    [Range(0f, 1f)]
    public float AmourPercent = 0.2f;

    [Header("Health Percent")]
    [Range(0f, 1f)]
    public float HealthPercent = 0.15f;

    [Header("Health Currently")]
    public float HealthRecovery = 2f;

    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.DamageRatio -= RatioDamagePercent;
        configurator.PlayerMaxAmour += configurator.PlayerMaxAmour * AmourPercent;
        configurator.PlayerMaxHealth += configurator.PlayerMaxHealth * HealthPercent;
        configurator.HealthRecovery += HealthRecovery;
    }
}

