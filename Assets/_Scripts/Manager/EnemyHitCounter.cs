using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHitCounter : MonoBehaviour
{
    public static EnemyHitCounter Instance;

    private int enemyHitCount = 0;
    private int enemyHitCountForElement = 0;
    private int Element = 0;
    private int Explosive = 0;
    private bool CountElement = false;
    private bool isElementCount = false;
    private bool isExplosive = false;
    private bool isCount = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        isCount = true;
    }
    // Normal Shot
    public void RegisterEnemyHit()
    {
        if (isCount)
        { enemyHitCount++; }
    }
    public void StartCount() => isCount = true;
    public void StopCount() => isCount = false;
    public int GetEnemyHitCount()
    {
        return enemyHitCount;
    }
    public void ResetCounter()
    {
        enemyHitCount = 0;
    }


    // Element Count
    public void RegisterElementHit()
    {
        if(isElementCount)
        {
            enemyHitCountForElement++;
            Debug.Log("Enemy Element Hit Count: " + enemyHitCount);
        }    
        
    }

    public void StartElementCount()
    {
        isElementCount = true;
        ResetCounterElement();
    }    

    public void StopElementCount()
    {
        isElementCount = false;
    }    

    public int GetElementEnemyHitCount()
    {
        return enemyHitCountForElement;
    }

    public void ResetCounterElement()
    {
        enemyHitCountForElement = 0;
    }

    public void ElementShot()
    {
        if (CountElement)
        {
            Element++;
        }

    }
    public void StartCountElement()
    {
        CountElement = true;
        ResetCountElement();
    }

    public void StopCountElement()
    {
        CountElement = false;
    }
    public int GetElementEnemyCount()
    {
        return Element;
    }

    public void ResetCountElement()
    {
        Element = 0;
    }

    //Explosive Shot

    public void ExplosiveCount()
    {
        if(isExplosive) Explosive++;

    }
    public void StartCountExplosive()
    {
        isExplosive = true;
        ResetExplosiveCount();
    }
    public void StopCountExplosive()
    {
        isExplosive= false;
    }
    public int GetExplosiveCount() { return Explosive; }
    public void ResetExplosiveCount() { Explosive = 0; }

}
