using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/QuestData")]
public class QuestData : ScriptableObject
{
    public string questName;
    [TextArea] public string description;
    public AudioClip questAudio;

    public TaskType taskType;
    public TaskID taskID;

    [HideInInspector] public bool isCompleted = false;
}
public enum TaskType
{
    MainTask,
    SubTask
}
public enum TaskID
{
    None = 0,
    MainTask1,
    MainTask2,
    MainTask3,
    MainTask4,
    MainTask5,
    MainTask6,
    MainTask7,
    MainTask8,
    MainTask9,
    MainTask10,
    SubTask1,
    SubTask2,
    SubTask3,
    SubTask4,
    SubTask5,
    SubTask6,
    SubTask7,
    SubTask8,
    SubTask9,
    SubTask10,
    SubTask11,
    SubTask12,
    SubTask13,
    SubTask14,
    SubTask15,
    SubTask16,
    SubTask17,
    SubTask18,
    SubTask19,
    SubTask20
}
