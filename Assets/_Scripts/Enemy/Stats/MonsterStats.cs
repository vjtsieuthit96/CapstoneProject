using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    [SerializeField] private MonsterStatsSO statsSO;

    private float _currentHealth;
    private float _currentDefense;
    private float _currentDamage;

    private void Start()
    {
        SetDefaultStats();
        _currentHealth = statsSO.GetMaxHealth();      
    }
   
    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => statsSO.GetMaxHealth();
    public void AddDefensePercent(float percent) => _currentDefense += percent/100 * _currentDefense;   
    public void AddDamagePercent(float percent) => _currentDamage += percent/100 * _currentDamage;
    public void SetDefaultStats()
    {
        _currentDefense = statsSO.GetDefense();
        _currentDamage = statsSO.GetDamage();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage - (damage * _currentDefense / 100);
        _currentHealth = Mathf.Max(_currentHealth, 0);
    }

}