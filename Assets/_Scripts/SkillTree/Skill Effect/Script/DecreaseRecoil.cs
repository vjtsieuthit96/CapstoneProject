using UnityEngine;
[CreateAssetMenu(fileName = "Decrease Recoil", menuName = "Scriptable Objects/AllGun/Decrease Recoil")]
public class DecreaseRecoil : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float recoilPercent;

    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.GunRecoil -= (configurator.GunRecoil * recoilPercent);
    }
}
