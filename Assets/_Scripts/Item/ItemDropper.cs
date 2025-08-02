using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public ItemDropCollection dropTable;

    public void TryDropItem()
    {
        if (dropTable == null || dropTable.possibleDrops.Count == 0) return;

        float roll = Random.value;
        if (roll > dropTable.overallDropChance) return;

        float totalWeight = 0f;
        foreach (var entry in dropTable.possibleDrops)
        {
            totalWeight += entry.dropRate;
        }

        float randomPoint = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var entry in dropTable.possibleDrops)
        {
            cumulative += entry.dropRate;
            if (randomPoint <= cumulative)
            {
                GameObject item = ItemPoolManager.Instance.GetFromPool(entry.itemPrefab);
                if (item != null)
                {
                    item.transform.position = transform.position;
                    item.transform.rotation = Quaternion.identity;
                    item.SetActive(true);

                    // Gán prefab gốc để biết đường trả lại pool đúng
                    var pickup = item.GetComponent<ItemPickup>();
                    if (pickup != null)
                    {
                        pickup.SetOrigin(entry.itemPrefab);
                    }
                }
                return;
            }
        }
    }
}
