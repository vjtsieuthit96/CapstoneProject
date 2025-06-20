using UnityEngine;

[CreateAssetMenu(fileName = "TrueDamagenHealthRecovery", menuName = "Scriptable Objects/DefenseHealth/TrueDamagenHealthRecovery")]
public class TrueDamagenHealthRecovery : SkillEffect
{
    [Header("Damage Ratio Percent")]
    [Range(0f, 1f)]
    public float RatioDamagePercent = 0.07f;
    [Header("Health Recovery Percent")]
    [Range(0f, 1f)]
    public float HealthRecoveryperTime = 0.5f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.HealthRecoveryPerTime -= configurator.HealthRecoveryPerTime * HealthRecoveryperTime;
        configurator.DamageRatio -= RatioDamagePercent;

    }
}
