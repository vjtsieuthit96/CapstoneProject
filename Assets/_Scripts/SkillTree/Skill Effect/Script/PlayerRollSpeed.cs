using UnityEngine;
[CreateAssetMenu(fileName = "PlayerRollSpeed", menuName = "Scriptable Objects/Strength/PlayerRollSpeed")]
public class PlayerRollSpeed : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float RollSpeedPercent = 0.1f;

    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.rollSpeed += configurator.rollSpeed * RollSpeedPercent;
    }
}
