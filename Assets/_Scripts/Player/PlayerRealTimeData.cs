using UnityEngine;

public class PlayerRealTimeData : MonoBehaviour
{
    public static PlayerRealTimeData Instance { get; private set; }

    public SkillTreeState currentSkillTreeState = new SkillTreeState();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
