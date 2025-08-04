using UnityEngine;

[CreateAssetMenu(fileName = "AmourSkillTree", menuName = "Scriptable Objects/DefenseHealth/AmourSkillTree")]
public class AmourSkillTree : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float AmourPercent = 0.1f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        Debug.Log("Apply Effect");
        configurator.PlayerMaxAmour += configurator.PlayerMaxAmour * AmourPercent;
    }
}