using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class EnemyKillEntry
{
    public string enemyType;
    public int killCount;
}
public class PlayerPlayRecords : MonoBehaviour
{
    [Header("Tổng số Enemy tiêu diệt")]
    [SerializeField] 
    private int totalKills = 0;

    [Header("Các loại Enemy tiêu diệt")]
    [SerializeField]
    private List<EnemyKillEntry> enemyKillList = new List<EnemyKillEntry>();

    public void RegisterKill(string enemyType)
    {
        totalKills++;

        var entry = enemyKillList.Find(e => e.enemyType == enemyType);
        if (entry != null)
        {
            entry.killCount++;
        }
        else
        {
            enemyKillList.Add(new EnemyKillEntry
            {
                enemyType = enemyType,
                killCount = 1
            });
        }

        Debug.Log($"[Kill] {enemyType} => {GetKillCount(enemyType)} (Total: {totalKills})");
    }
    public int GetTotalKills() => totalKills;

    public int GetKillCount(string enemyType)
    {
        var entry = enemyKillList.Find(e => e.enemyType == enemyType);
        return entry != null ? entry.killCount : 0;
    }
}
