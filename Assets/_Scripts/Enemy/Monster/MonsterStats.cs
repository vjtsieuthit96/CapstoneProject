using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _healingRate;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _defense;

    private void Start()
    {
        _currentHealth = _maxHealth;
        StartCoroutine(Healing());
    }

    public float GetCurrentHealth() => _currentHealth;

    public void SetHealingRate(float rate) => _healingRate = rate;    

    IEnumerator Healing()
    {
        while (_currentHealth < _maxHealth)
        {
            _currentHealth += _healingRate * Time.deltaTime;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            yield return new WaitForSecondsRealtime(1);
        }
    }
    
    public float GetDamage() => _damage;
    public void TakeDamage(float damage)
    {
        _currentHealth = _currentHealth - (damage - damage * _defense / 100);
    }

    public float GetSpeed() => _speed;

    public float GetDefense() => _defense;
    
}
