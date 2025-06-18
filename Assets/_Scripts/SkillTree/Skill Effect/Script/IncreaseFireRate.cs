using UnityEngine;
[CreateAssetMenu(fileName = "AllGun", menuName = "Scriptable Objects/FireRate/AllGuns")]

public class IncreaseFireRate : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float FireRate;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.PlayerFireRate -= (configurator.PlayerFireRate * FireRate);
    }
}
