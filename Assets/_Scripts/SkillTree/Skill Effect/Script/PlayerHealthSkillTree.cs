using UnityEngine;
[CreateAssetMenu(fileName = "PlayerHealth", menuName = "Scriptable Objects/Strength/PlayerHealth")]
public class PlayerHealthSkillTree : SkillEffect
{
    [Header("Percent")]
    [Range(0f,1f)]
    public float HealthIncreasePercent = 0.05f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.sprintSpeed += configurator.sprintSpeed * HealthIncreasePercent;
        configurator.runSpeed += configurator.runSpeed * HealthIncreasePercent;
        configurator.walkSpeed += configurator.walkSpeed * HealthIncreasePercent;
        configurator.freeMovementAnimatorSpeed += configurator.freeMovementAnimatorSpeed * HealthIncreasePercent;
    }
}
