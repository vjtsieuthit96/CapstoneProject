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
                Instantiate(entry.itemPrefab, transform.position, Quaternion.identity);
                return;
            }
        }
    }
}
