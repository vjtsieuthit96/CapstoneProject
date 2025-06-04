using UnityEngine;

public class BloodAttackEffect : MonoBehaviour
{
    [SerializeField] private BloodEffect bloodPrefabs; // 🔥 Danh sách các hiệu ứng máu

    private void Start()
    {            
        PoolManager.Instance.CreatePool("BloodEF", bloodPrefabs, 5);      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PoolManager.Instance.GetObject<BloodEffect>("BloodEF", transform.position, Quaternion.identity);
        }
    }
}