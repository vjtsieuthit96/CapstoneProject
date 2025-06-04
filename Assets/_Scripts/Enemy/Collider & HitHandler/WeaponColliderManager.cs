using UnityEngine;

public class WeaponColliderManager : MonoBehaviour
{
    [SerializeField] private GameObject bloodPrefabs;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Take Hit");
            Instantiate(bloodPrefabs, transform.position, Quaternion.identity);
        }
    }
}
