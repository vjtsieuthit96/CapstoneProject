using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string PlayerName;
    public float PlayerDefaultSpeed; // tốc độ di chuyển thường
    public float RunSpeed; // tốc độ chạy
    public float PlayerWeight; // ảnh hưởng sức bật, khối lượng tổng của nhân vật càng nặng thì khi di chuyển càng chậm, bật càng thấp và bobing đầu nhiều hơn
    public float PlayerMaxHealth;
    public float PlayerStrength; // áp dụng cho sức bật và tấn công vật lí 
    public Gender PlayerGender;
}

public enum Gender
{
    male, female
}
