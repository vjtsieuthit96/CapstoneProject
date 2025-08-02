using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDropCollection", menuName = "Items/Drop Collection")]
public class ItemDropCollection : ScriptableObject
{
    [Range(0f, 1f)] public float overallDropChance = 0.5f;
    public List<ItemDropPercent> possibleDrops;
}