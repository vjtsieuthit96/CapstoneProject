using UnityEngine;
[CreateAssetMenu(fileName = "HealthSkillTree", menuName = "Scriptable Objects/DefenseHealth/HealthSkillTree")]
public class HealthSkillTree : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float HealthPercent = 0.07f;

    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.PlayerMaxHealth += configurator.PlayerMaxHealth * HealthPercent;
    }
}



