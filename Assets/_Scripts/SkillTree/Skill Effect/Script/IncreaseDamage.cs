using UnityEngine;

[CreateAssetMenu(fileName = "Increase Damage", menuName = "Scriptable Objects/SkillEffects/Damage")]
public class IncreaseDamage : SkillEffect
{
    [Header("Percent")]
    public float damageIncrease;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {

        configurator.PlayerDamageMultiplierLonggun += (configurator.PlayerDamageMultiplierLonggun * damageIncrease);
        configurator.PlayerDamageMultiplierShortgun += (configurator.PlayerDamageMultiplierShortgun * damageIncrease);
    }
}

