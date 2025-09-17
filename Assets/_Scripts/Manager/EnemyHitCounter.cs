using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHitCounter : MonoBehaviour
{
    public static EnemyHitCounter Instance;

    public int enemyHitCount = 0;
    public int enemyHitCountForElement = 0;
    public int Element = 0;
    public int Explosive = 0;
    public bool CountElement = false;
    public bool isElementCount = false;
    public bool isExplosive = false;
    public bool isCount = true;
    public int PlayerKill = 0;

    public List<EmotionSystem> Emotions;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        isCount = true;
    }

    // Normal Shot
    public void RegisterEnemyHit()
    {
        foreach (EmotionSystem system in Emotions)
        {
            if (system != null)
            {
                system.OnHitEnemy(0.02f);
            }
        }
        if (isCount)
        { 
            //foreach(EmotionSystem system in Emotions)
            //{
            //    if(system != null)
            //    {
            //        system.OnHitEnemy(0.2f);
            //    }    
            //}    
            enemyHitCount++; 
        }
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
