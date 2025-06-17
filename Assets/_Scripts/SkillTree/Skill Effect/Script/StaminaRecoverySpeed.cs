using UnityEngine;
[CreateAssetMenu(fileName = "Stamina Recovery Speed", menuName = "Scriptable Objects/Strength/StaminaRecovery")]

public class StaminaRecoverySpeed : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 2f)]
    public float StaminaRecoveryPercent = 0.3f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.staminaRecovery += configurator.staminaRecovery * StaminaRecoveryPercent;
    }
}
