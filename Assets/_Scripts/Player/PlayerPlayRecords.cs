using Unity.VisualScripting;
using UnityEngine;

public class PlayerPlayRecords : MonoBehaviour
{
    [SerializeField]private int PlayerKill = 0;
    public void PlayerCountKill()
    {
        PlayerKill++;
        Debug.Log("Player Kill: " + PlayerKill);
    }
}
