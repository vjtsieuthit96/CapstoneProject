using UnityEngine;
[CreateAssetMenu(fileName = "PlayerFallDamage", menuName = "Scriptable Objects/Strength/PlayerFallDamage")]
public class PlayerFallDamage : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float FallDamagePercent = 0.2f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.fallMinHeight += configurator.fallMinHeight * FallDamagePercent;
    }
}
