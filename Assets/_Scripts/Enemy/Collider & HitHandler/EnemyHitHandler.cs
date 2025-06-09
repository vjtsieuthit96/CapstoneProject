using Invector;
using UnityEngine;

public class EnemyHitHandler : MonoBehaviour
{
    private MonsterAI monsterAi;
    private float damageMultiplier; // Nhân sát thương nếu trúng vị trí đặc biệt

    public void Initialize(MonsterAI monsterAi, float multiplier)
    {
        this.monsterAi = monsterAi;
        this.damageMultiplier = multiplier;
    }

    public void ApplyHit(float bulletDamage)
    {
        float finalDamage = bulletDamage * damageMultiplier;
        monsterAi.ApplyDamage(finalDamage);
        Debug.Log($"Hit: {gameObject.name}, Damage: {finalDamage}");
        int rate = Random.Range(0, 100);
        if (rate <= 30 && !monsterAi.GetIsHit())
        {
            monsterAi.SetAnimatorParameter(MonsterAnimatorHash.takeHitHash, null);
            Debug.Log("Enemy get hit animation");
        }
    }

    public void ApplyFreeze(float duration)
    {
        monsterAi.FreezyEnemy(duration);
    }
}