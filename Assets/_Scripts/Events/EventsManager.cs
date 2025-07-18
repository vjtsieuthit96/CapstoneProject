using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public SkillTreePointEvents skillTreePointEvents;
    public static EventsManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("one or more object GameEventManager in scene");
        }
        Instance = this;
        skillTreePointEvents = new SkillTreePointEvents();
    }
}
