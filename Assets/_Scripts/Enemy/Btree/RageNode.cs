using UnityEngine;

public class RageNode : Node
{    
    private MonsterStats monsterStats;
    private MonsterAI monsterAI;
    [Header("-----Rage-----")]
    private float rageDuration = 240f; // Thời gian Cuồng Nộ
    private float rageCooldown = 600f; // Hồi chiêu Cuồng Nộ
    private float lastRageTime = -Mathf.Infinity; // Thời điểm kích hoạt Cuồng Nộ
    private bool isEnraged = false; // Trạng thái Cuồng Nộ

    public RageNode(MonsterAI monsterAI, MonsterStats monsterStats)
    { 
        this.monsterAI = monsterAI;
        this.monsterStats = monsterStats;
    }

    public override NodeState Evaluate()
    {
        float timeSinceLastRage = Time.time - lastRageTime;
        // Nếu Rage kết thúc, quái trở lại trạng thái bình thường
        if (isEnraged && timeSinceLastRage >= rageDuration)
        {
            isEnraged = false;
            monsterStats.SetDefaultStats(); // Đặt lại stats về mặc định
            Debug.Log("Enraged End");
            return NodeState.FAILURE;
        }
        // Nếu Rage cd xong, kích hoạt lại
        if (!isEnraged && timeSinceLastRage >= rageCooldown)
        {
            monsterAI.SetAnimatorParameter(MonsterAnimatorHash.roarHash, null);            
            isEnraged = true;
            lastRageTime = Time.time;
            Debug.Log("Rage State");
            monsterStats.AddDamagePercent(75f); // Tăng sát thương
            monsterStats.AddDefensePercent(75f); // Tăng phòng thủ
            return NodeState.SUCCESS;
        }
        
        return NodeState.FAILURE;
    }
}
