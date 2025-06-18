using UnityEngine;
[CreateAssetMenu(fileName = "PlayerAirSpeed", menuName = "Scriptable Objects/Strength/PlayerAirSpeed")]
public class PlayerAirspeed : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float speedPercent = 0.25f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.airSpeed += configurator.airSpeed * speedPercent;
    }
}
