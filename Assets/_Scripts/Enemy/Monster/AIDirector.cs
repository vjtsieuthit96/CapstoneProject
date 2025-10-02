using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Transform player;
    [SerializeField] private List<Transform> hotSpots;

    [Header("Settings")]
    [Tooltip("Khoảng cách tối đa để coi là gần hotspot và dừng spawn")]
    [SerializeField] private float stopDistance = 10f;

    [Tooltip("Số level tối đa (0 = không spawn, max = khó nhất)")]
    [SerializeField] private int maxLevel = 6;

    [Tooltip("Khoảng cách xa nhất để đạt maxLevel (vd: 100f)")]
    [SerializeField] private float maxDistance = 100f;

    [Header("Runtime")]
    public bool directorEnabled = true;

    private void Update()
    {
        if (!directorEnabled || spawner == null || player == null || hotSpots.Count == 0)
            return;
        float minDist = float.MaxValue;
        foreach (var point in hotSpots)
        {
            float dist = Vector3.Distance(player.position, point.position);
            if (dist < minDist)
                minDist = dist;
        }
        if (minDist <= stopDistance)
        {
            spawner.canSpawn = false;
            spawner.currentLevel = 0;
            return;
        }
        else
        {
            spawner.canSpawn = true;
        }

        int newLevel = Mathf.Clamp(Mathf.FloorToInt((1f - (minDist / maxDistance)) * maxLevel), 0, maxLevel);
        spawner.currentLevel = newLevel;
    }

    public void SetPlayer(Transform PlayertoSet)
    {
        player = PlayertoSet;
    }    

    public void DisableDirector()
    {
        directorEnabled = false;
    }

    public void EnableDirector()
    {
        directorEnabled = true;
    }
}
