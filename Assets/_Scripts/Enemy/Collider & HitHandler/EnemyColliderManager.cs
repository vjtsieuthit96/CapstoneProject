using UnityEngine;

public class EnemyColliderManager : MonoBehaviour
{
    [SerializeField] private MonsterAI monsterAi;
    [Header("-----Head Colldier-----")]
    [SerializeField] private Collider headCollider;
    [SerializeField] private float headshotMultiplier = 2.0f; // x damage trúng đầu
    [Header("-----Body Colldier-----")]
    [SerializeField] private Collider[] colliders;

    private void Awake()
    {
        // Xử lý các collider trên thân quái
        foreach (Collider col in colliders)
        {
            var bodyColliderHandler = col.gameObject.AddComponent<EnemyHitHandler>();
            bodyColliderHandler.Initialize(monsterAi, 1f);
        }

        // Xử lý riêng HeadCollider nếu tồn tại
        if (headCollider != null)
        {
            EnemyHitHandler headColliderHandler = headCollider.gameObject.AddComponent<EnemyHitHandler>();
            headColliderHandler.Initialize(monsterAi, headshotMultiplier);            
        }
        else
        {
            Debug.LogWarning("HeadCollider chưa được gán hoặc bị thiếu trong Inspector!");
        }
    }
}