using UnityEngine;

public class RageOrRetreatNode : Node
{    
    private MonsterStats monsterStats;
    [Header("-----Rage-----")]
    [SerializeField] private float rageDuration = 30f; // Thời gian Cuồng Nộ
    [SerializeField] private float rageCooldown = 600f; // Hồi chiêu Cuồng Nộ
    [SerializeField] private float lastRageTime = -Mathf.Infinity; // Thời điểm kích hoạt Cuồng Nộ
    [SerializeField] private bool isEnraged = false; // Trạng thái Cuồng Nộ

    public RageOrRetreatNode(MonsterStats monsterStats)
    { 
        this.monsterStats = monsterStats;
    }

    public override NodeState Evaluate()
    {
        float timeSinceLastRage = Time.time - lastRageTime;
        // Nếu Rage kết thúc, quái trở lại trạng thái bình thường
        if (isEnraged && timeSinceLastRage >= rageDuration)
        {
            isEnraged = false;
            Debug.Log("Enraged End");
            return NodeState.FAILURE;
        }
        // Nếu Rage cd xong, kích hoạt lại
        if (!isEnraged && timeSinceLastRage >= rageCooldown)
        {
            isEnraged = true;
            lastRageTime = Time.time;
            Debug.Log("Rage State");
            //Logic của cuồng nộ
            return NodeState.SUCCESS;
        }
        // Rage chưa hồi skill thì chạy thôi
        // Logic rút lui
        return NodeState.FAILURE;
    }
}
