using Invector;
using UnityEngine;

public class EnemyHitHandler : MonoBehaviour
{
    private MonsterAI monsterAi;
    private float damageMultiplier;//  Nhân sát thương nếu trúng vị trí đặc biệt

    public void Initialize(MonsterAI monsterAi, float multiplier)
    {
        this.monsterAi = monsterAi;
        this.damageMultiplier = multiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) //  Kiểm tra nếu va chạm với đạn
        {
            vDestroyGameObject bullet = other.GetComponent<vDestroyGameObject>();
            if (bullet != null)
            {
                float finalDamage = bullet.Damage * damageMultiplier;
                monsterAi.ApplyDamage(finalDamage);
                Debug.Log($"Hit: {gameObject.name}, Damage: {finalDamage}");
            }
            int Rate = Random.Range(0, 100);
            if (Rate <= 30)
            {
                monsterAi.SetAnimatorParameter(MonsterAnimatorHash.takeHitHash, null);
                Debug.Log("Enemy get hit animation");
            }
        }
    }
}