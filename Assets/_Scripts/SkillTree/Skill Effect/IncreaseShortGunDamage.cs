using UnityEngine;

[CreateAssetMenu(fileName = "ShortGun", menuName = "Scriptable Objects/Damage/ShortGun")]
public class IncreaseShortGunDamage : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float gunDamage;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.PlayerDamageMultiplierShortgun += (configurator.PlayerDamageMultiplierShortgun * gunDamage);

    }
}
