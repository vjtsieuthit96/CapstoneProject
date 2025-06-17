using UnityEngine;

public class PlayerStamina : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float staminaIncreasePercent = 0.1f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.maxStamina += configurator.maxStamina * staminaIncreasePercent;
    }
}
