using Firebase.Database;
using Firebase;
using UnityEngine;
using Firebase.Extensions;


public class PlayerData
{
    public int Health;
    public int Energy;
}
public class FirebaseDatabaseManager : MonoBehaviour
{
    private DatabaseReference reference;
    public int totalCoins;

    private void Awake()
    {

        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
    }

    // ghi dữ liệu 
    public void WriteDatabase(string id, int health, int energy)// string id, string message
    {
        var playerData = new
        {
            Health = health,
            Energy = energy
        };

        string json = JsonUtility.ToJson(playerData);
        reference.Child("Users").Child(id).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        //reference.Child("Users").Child(id).Child("Gold").SetValueAsync(message).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(" Ghi dữ liệu thành công :)))))");
            }
            else
            {
                Debug.Log(" Ghi dữ liệu thất bại :((((( : " + task.Exception);
            }
        });
    }

    //public void ReadDatabase(string id)
    //{
    //    reference.Child("Users").Child(id).Child("Gold").GetValueAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsCompleted)
    //        {
    //            DataSnapshot snapshot = task.Result;
    //            Debug.Log(" Đọc dữ liệu thành công :))))) : " + snapshot.Value.ToString());
    //            totalCoins = Convert.ToInt32(snapshot.Value.ToString());
    //            Debug.Log(" Số vàng lấy  ra được : " + totalCoins);
    //        }
    //        else
    //        {
    //            Debug.Log(" Đọc dữ liệu thất bại :((((( : " + task.Exception);
    //        }
    //    });
    //}


    public void ReadDatabase(string id)
    {
        reference.Child("Users").Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                    Debug.Log("Đọc dữ liệu thành công :)))))");
                    Debug.Log($" Sức khỏe: {playerData.Health}, Năng lượng: {playerData.Energy}");
                }
                else
                {
                    Debug.Log("Người chơi không tồn tại.");
                }
            }
            else
            {
                Debug.Log("Đọc dữ liệu thất bại :((((( : " + task.Exception);
            }
        });
    }
}
