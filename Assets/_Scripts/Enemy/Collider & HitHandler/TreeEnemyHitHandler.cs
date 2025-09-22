using System.Collections;
using UnityEngine;

public class TreeEnemyHitHandler : EnemyHitHandler
{
    private float health = 100f;
    private bool isFrozen;
    private bool isShocked;

    public override void ApplyHit(float bulletDamage, GameObject player = null)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();

        health -= bulletDamage;
        Debug.Log($"[DummyEnemy] Take Damage: {bulletDamage} | Remaining HP: {health}");

        if (health <= 0f)
        {
            Debug.Log("[DummyEnemy] Dead!");
        }
    }

    public override void ApplyPoisonDamage(float damage, float duration, GameObject player = null)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        StartCoroutine(DummyPoisonCoroutine(damage, duration));
    }

    private IEnumerator DummyPoisonCoroutine(float damage, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            health -= damage;
            Debug.Log($"[DummyEnemy] Poison tick: {damage} | Remaining HP: {health}");
            yield return new WaitForSeconds(3f);
            elapsed += 3f;
        }
    }

    public override void ApplyFreeze(float duration)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        isFrozen = true;
        Debug.Log($"[DummyEnemy] Frozen for {duration}s");
        StartCoroutine(RemoveFreeze(duration));
    }

    private IEnumerator RemoveFreeze(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        Debug.Log("[DummyEnemy] Freeze ended");
    }

    public override void ApplySlowDown(float percent, float duration)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        Debug.Log($"[DummyEnemy] Slowed by {percent * 100}% for {duration}s");
    }

    public override void ApplyShock(float duration)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        isShocked = true;
        Debug.Log($"[DummyEnemy] Shocked for {duration}s");
        StartCoroutine(RemoveShock(duration));
    }

    private IEnumerator RemoveShock(float duration)
    {
        yield return new WaitForSeconds(duration);
        isShocked = false;
        Debug.Log("[DummyEnemy] Shock ended");
    }

    public override void ApplyBleed(Vector3 position)
    {
        EnemyHitCounter.Instance.RegisterEnemyHit();
        Debug.Log($"[DummyEnemy] Bleeding at position {position}");
    }
}
