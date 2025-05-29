using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStatsSO", menuName = "Scriptable Objects/MonsterStatsSO")]
public class MonsterStatsSO : ScriptableObject
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _damage;
    [SerializeField] private float _defense;

    // Getter cho các chỉ số
    public float GetMaxHealth() => _maxHealth;
    public float GetDamage() => _damage;
    public float GetDefense() => _defense; // % dame sẽ giảm vd 10 là sẽ giảm 10% dame nhận vào
}