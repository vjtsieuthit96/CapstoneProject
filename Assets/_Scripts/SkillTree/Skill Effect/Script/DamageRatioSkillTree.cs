using UnityEngine;

[CreateAssetMenu(fileName = "DamageRatioSkillTree", menuName = "Scriptable Objects/DefenseHealth/DamageRatioSkillTree")]
public class DamageRatioSkillTree : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float RatioDamagePercent = 0.03f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.DamageRatio -= RatioDamagePercent;
    }
}
