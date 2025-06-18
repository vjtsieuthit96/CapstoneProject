using UnityEngine;
[CreateAssetMenu(fileName = "Reload Speed", menuName = "Scriptable Objects/AllGun/Reload Speed")]
public class IncreaseReloadSpeed : SkillEffect
{
    [Header("Percent")]
    [Range(0f,1f)]
    public float reloadSpeedPercent;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.ReloadSpeed -= (configurator.ReloadSpeed * reloadSpeedPercent );
    }
}
