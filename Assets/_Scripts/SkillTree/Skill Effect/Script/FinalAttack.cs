using UnityEngine;

[CreateAssetMenu(fileName = "FinalAttack", menuName = "Scriptable Objects/AllGun/FinalAttack")]
public class FinalAttack : SkillEffect
{
    [Range(0f, 1f)] public float longGunBonusPercent = 0.2f;
    [Range(0f, 1f)] public float reloadSpeedBonusPercent = 0.3f;
    [Range(0f, 1f)] public float finalBulletBonusPercent = 0.3f;
    public int killStreakToAddAmmo = 7;
    public int bulletToAdd = 10;

    private bool bonusApplied = false;
    private bool finalBulletBonusActive = false;
    private int lastKillCheckpoint = 0;

    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        if (!bonusApplied)
        {
            configurator.PlayerDamageMultiplierLonggun += configurator.PlayerDamageMultiplierLonggun * longGunBonusPercent;
            configurator.ReloadSpeed += configurator.ReloadSpeed * reloadSpeedBonusPercent;
            bonusApplied = true;
        }
    }

    public override void UpdateCondition(CharacterConfigurator configurator)
    {
        if (configurator.CurrentBullet == 1 && !finalBulletBonusActive)
        {
            configurator.PlayerDamageMultiplierLonggun += configurator.PlayerDamageMultiplierLonggun * finalBulletBonusPercent;
            configurator.PlayerDamageMultiplierShortgun += configurator.PlayerDamageMultiplierShortgun * finalBulletBonusPercent;
            finalBulletBonusActive = true;
            Debug.Log($"{nameof(FinalAttack)}: Final bullet bonus activated");

        }
        else if (configurator.CurrentBullet > 1 && finalBulletBonusActive)
        {
            configurator.PlayerDamageMultiplierLonggun /= (1 + finalBulletBonusPercent);
            configurator.PlayerDamageMultiplierShortgun /= (1 + finalBulletBonusPercent);
            finalBulletBonusActive = false;
            Debug.Log($"{nameof(FinalAttack)}: Final bullet bonus deactivated");

        }
        int currentHitCount = EnemyHitCounter.Instance.GetEnemyHitCount();
        if (currentHitCount != 0 && currentHitCount % killStreakToAddAmmo == 0 && currentHitCount != lastKillCheckpoint)
        {
            configurator.AddBullet(bulletToAdd);
            lastKillCheckpoint = currentHitCount;
            Debug.Log($"{nameof(FinalAttack)}: Activate");

        }
    }
}
