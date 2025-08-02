// DropData/Logic
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemEffect effectToApply;
    private GameObject originPrefab;

    public void SetOrigin(GameObject prefab)
    {
        originPrefab = prefab;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            string rootName = other.transform.root.gameObject.name;
            ItemEffectApplier applier = other.GetComponentInParent<ItemEffectApplier>();
            if (applier != null)
            {
                applier.ApplyEffect(effectToApply, rootName);
                ReturnToPool();
            }
        }
    }

    private void ReturnToPool()
    {
        if (originPrefab != null)
        {
            ItemPoolManager.Instance.ReturnToPool(originPrefab, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
