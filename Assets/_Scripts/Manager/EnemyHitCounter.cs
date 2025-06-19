using UnityEngine;

public class EnemyHitCounter : MonoBehaviour
{
    public static EnemyHitCounter Instance;

    private int enemyHitCount = 0;
    private int enemyHitCountForElement = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEnemyHit()
    {
        enemyHitCount++;
        Debug.Log("Enemy Hit Count: " + enemyHitCount);
    }

    public int GetEnemyHitCount()
    {
        return enemyHitCount;
    }

    public void ResetCounter()
    {
        enemyHitCount = 0;
    }
    public void RegisterElementEnemyHit()
    {
        enemyHitCountForElement++;
        Debug.Log("Enemy Elements Hit Count: " + enemyHitCountForElement);
    }

    public int GetElementEnemyHitCount()
    {
        return enemyHitCountForElement;
    }

    public void ResetCounterElement()
    {
        enemyHitCountForElement = 0;
    }
}
