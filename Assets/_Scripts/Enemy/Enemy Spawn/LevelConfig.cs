[System.Serializable]
public class LevelConfig
{
    public int level;
    public int totalPoints;
    public int maxEnemyCount;
    public LevelType levelType;
}

public enum LevelType
{
    None,
    Attrition,      // Rỉa máu, cáu tài nguyên của người -> sử dụng để chơi cm thằng player
    Hold,           // Giữ chân người chơi, làm chậm tiến độ thực hiện nhiệm vụ của thằng người chơi
    Assault         // Tổng lực tấn công
}
