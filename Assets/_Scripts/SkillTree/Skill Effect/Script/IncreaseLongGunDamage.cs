using UnityEngine;

[CreateAssetMenu(fileName = "LongGun", menuName = "Scriptable Objects/Damage/LongGun")]
public class IncreaseLongGunDamage : SkillEffect
{
    [Header("Percent")]
    [Range(0f,1f)]
    public float gunDamage;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.PlayerDamageMultiplierLonggun += (configurator.PlayerDamageMultiplierLonggun * gunDamage);
    }
}
