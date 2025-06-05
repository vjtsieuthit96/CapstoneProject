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
        foreach (Collider col in colliders)
        {
            var colliderHandler = col.gameObject.AddComponent<EnemyHitHandler>();
            colliderHandler.Initialize(monsterAi, col == headCollider ? headshotMultiplier : 1.0f);
        }
    }
}