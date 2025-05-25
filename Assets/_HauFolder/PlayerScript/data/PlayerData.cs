using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Tooltip("Tên người chơi")]
    public string PlayerName;
    [Tooltip("Tốc độ di chuyển người chơi")]
    public float PlayerDefaultSpeed; // tốc độ di chuyển thường
    [Tooltip("Hệ số tỉ lệ giữa đi bộ và chạy")]
    public float RatioRun;// hệ số di chuyển giữa đi bộ và chạy
    [Tooltip("Ảnh hưởng sức bật, khối lượng tổng của nhân vật càng nặng thì khi di chuyển càng chậm, bật càng thấp và bobing đầu nhiều hơn")]
    public float PlayerWeight; // ảnh hưởng sức bật, khối lượng tổng của nhân vật càng nặng thì khi di chuyển càng chậm, bật càng thấp và bobing đầu nhiều hơn
    [Tooltip("Máu tối đa của người chơi")]
    public float PlayerMaxHealth;
    [Tooltip("Sức khỏe của người chơi")]
    [Range(2f,10f)]
    public float PlayerStrength; // áp dụng cho sức bật và tấn công vật lí 
    [Tooltip("Độ mượt của chuột")]
    public float PlayerSensity;
    [Tooltip("Giới tính nhân vật")]
    public Gender PlayerGender;
}

public enum Gender
{
    male, female
}
