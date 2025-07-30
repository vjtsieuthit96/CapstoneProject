using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemEffect effectToApply;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            string rootName = other.transform.root.gameObject.name;
            Debug.Log(message: "Va chạm Player..." + rootName);
            ItemEffectApplier applier = other.GetComponentInParent<ItemEffectApplier>();
            if (applier != null)
            {
                applier.ApplyEffect(effectToApply, rootName);
                Destroy(gameObject);
            }   
        }
    }
}
