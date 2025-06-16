using UnityEngine;

[CreateAssetMenu(fileName = "RestoreOnHit", menuName = "Scriptable Objects/AllGun/RestoreOnHit")]
public class AmmoRestoreOnHit : SkillEffect
{
    private int lastProcessedHitCount = -1;
    public int BullettoAdd = 3;

    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        lastProcessedHitCount = -1;
    }

    public override void UpdateCondition(CharacterConfigurator configurator)
    {
        int currentHit = EnemyHitCounter.Instance.GetEnemyHitCount();

        if (currentHit > 0 && currentHit % 5 == 0 && currentHit != lastProcessedHitCount)
        {
            configurator.AddBullet(BullettoAdd);
            lastProcessedHitCount = currentHit;
            Debug.Log($"{nameof(AmmoRestoreOnHit)}: Activate");
        }
    }
}
