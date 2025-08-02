using UnityEngine;

public class ItemDropPercent
{
    public GameObject itemPrefab;
    [Range(0f, 1f)] public float dropRate = 0.1f;
}
