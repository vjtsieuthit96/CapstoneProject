using UnityEngine;

[CreateAssetMenu(fileName = "HealthRecoverySpeed", menuName = "Scriptable Objects/DefenseHealth/HealthRecoverySpeed")]
public class HealthRecoverySpeed : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float HealthRecoveryperTime = 0.1f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.HealthRecoveryPerTime -= configurator.HealthRecoveryPerTime * HealthRecoveryperTime;
    }
}
