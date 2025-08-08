using Unity.VisualScripting;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private BossHealthBar bossHealthBar;
    [SerializeField] private Canvas bossCanvnas;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bossCanvnas.enabled = true;
            bossHealthBar.HealthBarOn();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bossCanvnas.enabled = false;           
        }
    }

}



