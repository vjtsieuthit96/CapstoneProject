using UnityEngine;

public class DarkCircleEffect : MonoBehaviour
{
    [Header("Damage value của skill")]
    public float damageValueEnter = 10f;
    public float damageValueStay = 10f;


    private void OnTriggerEnter(Collider other)
    {
        TryDamagePlayer(other, damageValueEnter);
    }

    private void OnTriggerStay(Collider other)
    {
        TryDamagePlayer(other, damageValueStay);
    }

    private void TryDamagePlayer(Collider other, float damavalue)
    {
        if (other == null) return;

        Transform playerTransform = other.transform;
        while (playerTransform != null)
        {
            if (playerTransform.CompareTag("Player"))
            {
                CharacterConfigurator cc = playerTransform.GetComponent<CharacterConfigurator>();
                if (cc != null)
                {
                    cc.TakeDamage(damavalue);
                }
                break;
            }
            playerTransform = playerTransform.parent;
        }
    }
}
