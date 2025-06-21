using Invector;
using UnityEngine;
using System.Collections;

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
        int rate = Random.Range(0, 100);      
        if (rate <= 30 && !monsterAi.GetIsHit())
        {
            monsterAi.SetAnimatorParameter(MonsterAnimatorHash.takeHitHash, null);
        }
    }
    public void ApplyPoisonDamage(float damage, float duration)
    {
        StartCoroutine(DamageOverTimeCoroutine(damage, duration));
    }
    private IEnumerator DamageOverTimeCoroutine(float damage, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            ApplyHit(damage);
            Debug.Log("Take Poison Damage: " + damage);
            yield return new WaitForSeconds(3f);
            elapsed += 3f;
        }
    }
    public void ApplyFreeze(float duration)
    {
        monsterAi.FreezyEnemy(duration);
    }
    public void ApplySlowDown(float percent, float duration)
    {
        monsterAi.SlowDown(percent, duration);
    }
    public void ApplyShock(float duration)
    {
        monsterAi.ShockEffect(duration);
    }
    public void ApplyBleed(Vector3 position)
    {
        monsterAi.BleedEffect(position);
    }
}