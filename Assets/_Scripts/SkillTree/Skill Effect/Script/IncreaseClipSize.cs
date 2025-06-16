using UnityEngine;
[CreateAssetMenu(fileName = "LongGunClipSize", menuName = "Scriptable Objects/Bullet/LongGunClipSize")]

public class IncreaseClipSize : SkillEffect
{
    [Header("Int Number")]
    public int ExtraBullet;

    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.LongGunClipSize += ExtraBullet;
    }
}
