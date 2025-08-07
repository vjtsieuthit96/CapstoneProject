using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    [SerializeField] private MonsterStatsSO statsSO;

    public float baseHealth;
    public float baseDefense;
    public float baseDamage;

    [SerializeField]private float _currentHealth;
    [SerializeField]private float _currentDefense;
    [SerializeField]private float _currentDamage;
    public string enemyType;
    private void Awake()
    {
        enemyType = statsSO.enemyType;
    }
    private void Start()
    {
        SetDefaultStats();
        _currentHealth = statsSO.GetMaxHealth(); 
        baseHealth = _currentHealth;
        baseDefense = _currentDefense;
        baseDamage = _currentDamage;
    }
   
    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => statsSO.GetMaxHealth();
    public float GetCurrentDamage() => _currentDamage;
    public float GetCurrentDefense() => _currentDefense;
    public void AddDefensePercent(float percent) => _currentDefense += percent/100 * _currentDefense;   
    public void AddDamagePercent(float percent) => _currentDamage += percent/100 * _currentDamage;

    public void ResetStatsToInitial()
    {
        _currentHealth = baseHealth;
        _currentDefense = baseDefense;
        _currentDamage = baseDamage;
    }


    public void SetDefaultStats()
    {
        _currentDefense = statsSO.GetDefense();
        _currentDamage = statsSO.GetDamage();
    }

    public void TakeDamage(float damage)
    {
        float finaldamage = damage - (damage * _currentDefense / 100);
        Debug.Log("Final damage monster take = " + finaldamage);
        _currentHealth -= finaldamage;
        _currentHealth = Mathf.Max(_currentHealth, 0);
    }

}