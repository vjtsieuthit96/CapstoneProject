using UnityEngine;

[CreateAssetMenu(fileName = "HealthAmour", menuName = "Scriptable Objects/DefenseHealth/HealthAmour")]
public class HealthAmour : SkillEffect
{
    [Header(" Amour Percent")]
    [Range(0f, 1f)]
    public float AmourPercent = 0.1f;

    [Header("Health Percent")]
    [Range(0f, 1f)]
    public float HealthPercent = 0.1f;

    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.PlayerMaxHealth += configurator.PlayerMaxHealth * HealthPercent;
        configurator.PlayerMaxAmour += configurator.PlayerMaxAmour * AmourPercent;
    }
}
