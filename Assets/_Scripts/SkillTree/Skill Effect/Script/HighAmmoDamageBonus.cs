using UnityEngine;

[CreateAssetMenu(fileName = "HighAmmoDamageBonus", menuName = "Scriptable Objects/AllGun/HighAmmoDamageBonus")]
public class HighAmmoDamageBonus : SkillEffect
{
    [Range(0f, 1f)]
    public float damageBonusPercent = 0.3f;

    private bool isActive = false;
    private float bonusShortgun = 0f;
    private float bonusLonggun = 0f;

    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        isActive = false;
        bonusShortgun = 0f;
        bonusLonggun = 0f;
    }

    public override void UpdateCondition(CharacterConfigurator configurator)
    {
        bool conditionMet = configurator.isHalfClip();

        if (conditionMet && !isActive)
        {
            bonusShortgun = configurator.PlayerDamageMultiplierShortgun * damageBonusPercent;
            bonusLonggun = configurator.PlayerDamageMultiplierLonggun * damageBonusPercent;

            configurator.PlayerDamageMultiplierShortgun += bonusShortgun;
            configurator.PlayerDamageMultiplierLonggun += bonusLonggun;

            isActive = true;
            Debug.Log($"{nameof(HighAmmoDamageBonus)} activated");
        }
        else if (!conditionMet && isActive)
        {
            configurator.PlayerDamageMultiplierShortgun -= bonusShortgun;
            configurator.PlayerDamageMultiplierLonggun -= bonusLonggun;

            isActive = false;
            bonusShortgun = 0f;
            bonusLonggun = 0f;
        }
    }
}
