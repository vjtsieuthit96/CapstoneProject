using System.Collections;
using UnityEngine;

public class ItemEffectApplier : MonoBehaviour
{
    public CharacterConfigurator stats;
    //Effect 
    [SerializeField] private ParticleSystem[] EffectParticle;
    [SerializeField] private Color survivalColor = Color.green;
    [SerializeField] private Color dopingColor = Color.blue;
    [SerializeField] private Color adrenalineColor = Color.yellow;
    [SerializeField] private Color berserkColor = Color.red;
    private void Awake()
    {
        stats = GetComponent<CharacterConfigurator>();
    }

    private void PlayerEffect(Color color)
    {
        foreach (var ps in EffectParticle)
        {
            var renderer = ps.GetComponent<Renderer>();
            if (renderer != null)
            {
                var mat = renderer.material;
                if (mat.HasProperty("_Color"))
                {
                    mat.color = color;
                }
                else if (mat.HasProperty("_TintColor"))
                {
                    mat.SetColor("_TintColor", color);
                }
                else if (mat.HasProperty("_BaseColor"))
                {
                    mat.SetColor("_BaseColor", color);
                }
            }

            ps.Play();
        }
    }

    public void ApplyEffect(ItemEffect effect, string PlayerName = "")
    {
        StartCoroutine(HandleEffect(effect,PlayerName));
    }

    private IEnumerator HandleEffect(ItemEffect effect, string PlayerName = "")
    {
        Debug.Log("Effect: " + effect.effectType);

        switch (effect.effectType)
        {
            case ItemEffectType.SurvivalMode:
                PlayerEffect(survivalColor);
                stats.HealthRecoveryPerTime *= 1f + effect.value;
                break;
            case ItemEffectType.AdrenalineRush:
                PlayerEffect(adrenalineColor);
                stats.walkSpeed *= 1f + effect.value;
                stats.runSpeed *= 1f + effect.value;
                stats.sprintSpeed *= 1f + effect.value;
                stats.crouchSpeed *= 1f + effect.value;
                stats.jumpHeight *= 1f + effect.value;
                stats.airSpeed *= 1f + effect.value;
                stats.airSmooth *= 1f + effect.value;
                stats.fallMinHeight *= 1f + effect.value;
                stats.rollSpeed *= 1f + effect.value;
                stats.rollRotationSpeed *= 1f + effect.value;
                stats.freeMovementAnimatorSpeed *= 1f + effect.value;
                break;
            case ItemEffectType.Doping:
                PlayerEffect(dopingColor);
                stats.maxStamina *= 1f + effect.value;
                stats.staminaRecovery *= 1f + effect.value;
                stats.sprintStamina *= 1f - effect.value;
                stats.jumpStamina *= 1f - effect.value;
                stats.rollStamina *= 1f - effect.value;
                break;
            case ItemEffectType.BerserkState:
                PlayerEffect(berserkColor);
                stats.DamageRatio /= 1f + effect.value;
                stats.PlayerShootingSpeed /= 1f + effect.value;
                stats.ReloadSpeed *= 1f + effect.value;
                stats.updateFireRate(1f + effect.value);
                break;
        }

        yield return new WaitForSeconds(effect.duration);

        switch (effect.effectType)
        {
            case ItemEffectType.SurvivalMode:
                stats.HealthRecoveryPerTime /= 1f + effect.value;
                break;
            case ItemEffectType.AdrenalineRush:
                stats.walkSpeed /= 1f + effect.value;
                stats.runSpeed /= 1f + effect.value;
                stats.sprintSpeed /= 1f + effect.value;
                stats.crouchSpeed /= 1f + effect.value;
                stats.jumpHeight /= 1f + effect.value;
                stats.airSpeed /= 1f + effect.value;
                stats.airSmooth /= 1f + effect.value;
                stats.fallMinHeight /= 1f + effect.value;
                stats.rollSpeed /= 1f + effect.value;
                stats.rollRotationSpeed /= 1f + effect.value;
                stats.freeMovementAnimatorSpeed /= 1f + effect.value;

                break;
            case ItemEffectType.Doping:
                stats.maxStamina /= 1f + effect.value;
                stats.staminaRecovery /= 1f + effect.value;
                stats.sprintStamina /= 1f - effect.value;
                stats.jumpStamina /= 1f - effect.value;
                stats.rollStamina /= 1f - effect.value;
                break;
            case ItemEffectType.BerserkState:
                stats.DamageRatio *= 1f + effect.value;
                stats.PlayerShootingSpeed *= 1f + effect.value;
                stats.ReloadSpeed /= 1f + effect.value;
                stats.updateFireRate(1/(1f + effect.value));
                break;
        }

        Debug.Log("Effect ended: " + effect.effectType);
    }
}
