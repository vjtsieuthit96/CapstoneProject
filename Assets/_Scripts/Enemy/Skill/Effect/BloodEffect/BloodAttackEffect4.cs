using UnityEngine;

public class BloodAttackEffect4 : MonoBehaviour
{
    [SerializeField] private BloodEffect4 bloodPrefabs;  

    private void Start()
    {            
        PoolManager.Instance.CreatePool("BloodEF4", bloodPrefabs, 20);      
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PoolManager.Instance.GetObject<BloodEffect4>("BloodEF4", transform.position, Quaternion.identity);
        }
    }    
}