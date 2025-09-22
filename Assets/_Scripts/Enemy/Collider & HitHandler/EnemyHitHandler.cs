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
    public virtual void ApplyHit(float bulletDamage, GameObject Player = null)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        float finalDamage = bulletDamage * damageMultiplier;

        if (Player != null)
        {
            monsterAi.RegisterDamage(Player, finalDamage);
        }

        monsterAi.ApplyDamage(finalDamage);

        int rate = Random.Range(0, 100);
        if (rate <= 30 && !monsterAi.GetIsHit())
        {
            monsterAi.SetAnimatorParameter(MonsterAnimatorHash.takeHitHash, null);
        }
    }
    public virtual void ApplyPoisonDamage(float damage, float duration, GameObject player = null)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        StartCoroutine(DamageOverTimeCoroutine(damage, duration, player));
    }
    private IEnumerator DamageOverTimeCoroutine(float damage, float duration, GameObject Player = null)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            ApplyHit(damage,Player);
            Debug.Log("Take Poison Damage: " + damage);
            yield return new WaitForSeconds(3f);
            elapsed += 3f;
        }
    }
    public virtual void ApplyFreeze(float duration)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        monsterAi.FreezyEnemy(duration);
    }
    public virtual void ApplySlowDown(float percent, float duration)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        monsterAi.SlowDown(percent, duration);
    }
    public virtual void ApplyShock(float duration)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        monsterAi.ShockEffect(duration);
    }
    public virtual void ApplyBleed(Vector3 position)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        monsterAi.BleedEffect(position);
    }
}