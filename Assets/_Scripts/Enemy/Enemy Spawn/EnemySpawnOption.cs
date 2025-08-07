using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnOption
{
    public EnemyData data;
    public int count;

    public static List<EnemySpawnOption> GetOptimalCombination(List<EnemyData> enemyDataList, int maxCount, int maxPoint)
    {
        List<EnemySpawnOption> bestCombo = new();
        int bestTotalPoint = 0;

        void Backtrack(int index, List<EnemySpawnOption> current, int usedCount, int usedPoint)
        {
            if (usedCount > maxCount || usedPoint > maxPoint) return;

            if (usedPoint > bestTotalPoint)
            {
                bestTotalPoint = usedPoint;
                bestCombo = CloneCombo(current);
            }
            else if (usedPoint == bestTotalPoint)
            {
                int currentEnemyCount = current.Sum(c => c.count);
                int bestEnemyCount = bestCombo.Sum(c => c.count);

                if (currentEnemyCount > bestEnemyCount)
                {
                    bestCombo = CloneCombo(current);
                }
            }
            for (int i = index; i < enemyDataList.Count; i++)
            {
                var enemy = enemyDataList[i];
                int maxCanAdd = Mathf.Min(
                    (maxPoint - usedPoint) / enemy.point,
                    maxCount - usedCount
                );

                for (int j = 1; j <= maxCanAdd; j++)
                {
                    current.Add(new EnemySpawnOption { data = enemy, count = j });
                    Backtrack(i + 1, current, usedCount + j, usedPoint + enemy.point * j);
                    current.RemoveAt(current.Count - 1);
                }
            }
        }

        List<EnemySpawnOption> CloneCombo(List<EnemySpawnOption> source)
        {
            return source.Select(c => new EnemySpawnOption
            {
                data = c.data,
                count = c.count
            }).ToList();
        }

        Backtrack(0, new List<EnemySpawnOption>(), 0, 0);
        return bestCombo;
    }

}