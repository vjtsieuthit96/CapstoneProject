using UnityEngine;
[CreateAssetMenu(fileName = "StrengthFinal", menuName = "Scriptable Objects/Strength/StrengthFinal")]
public class StrengthFinal : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float finalStatsIncreasePercent = 0.05f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        // Speed
        float speedpercent = finalStatsIncreasePercent * 4;
        configurator.sprintSpeed += configurator.sprintSpeed * speedpercent;
        configurator.runSpeed += configurator.runSpeed * speedpercent;
        configurator.walkSpeed += configurator.walkSpeed * speedpercent;
        configurator.freeMovementAnimatorSpeed += configurator.freeMovementAnimatorSpeed * speedpercent;

        // Roll speed
        float RollSpeedPercent = finalStatsIncreasePercent * 3;
        configurator.rollSpeed += configurator.rollSpeed * RollSpeedPercent;

        // Jump Force
        float jumpForce = finalStatsIncreasePercent * 2;
        configurator.jumpHeight += configurator.jumpHeight * jumpForce;
    }
}
