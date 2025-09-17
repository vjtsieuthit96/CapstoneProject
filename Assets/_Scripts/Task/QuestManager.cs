using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [SerializeField] private List<QuestData> mainTasks = new List<QuestData>();
    [SerializeField] private List<QuestData> subTasks = new List<QuestData>();

    [SerializeField] private QuestData currentMainTask;
    [SerializeField] private QuestData currentSubTask;

    public List<EmotionSystem> Emotions;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ReceiveQuest(QuestData questData)
    {
        if (questData.taskType == TaskType.MainTask)
        {
            mainTasks.Add(questData);
            if (currentMainTask == null || currentMainTask.isCompleted)
            {
                currentMainTask = questData;
                QuestUIManager.Instance.ShowTask(currentMainTask);
            }
        }
        else
        {
            subTasks.Add(questData);
            currentSubTask = questData;
            QuestUIManager.Instance.ShowTask(currentSubTask);
        }
    }

    public void CompleteTask(TaskID taskID)
    {
        foreach(EmotionSystem emotionSystem in Emotions)
        {
            if(emotionSystem != null)
            {
                emotionSystem.OnFinishMission(2f);
            }    
        }    
        QuestData task = FindTaskByID(taskID);
        if (task != null)
        {
            task.isCompleted = true;
            QuestUIManager.Instance.HideTask(task);

            if (task.taskType == TaskType.MainTask && task == currentMainTask)
            {
                currentMainTask = GetNextMainTask(taskID);
                if (currentMainTask != null)
                    QuestUIManager.Instance.ShowTask(currentMainTask);
            }
            else if (task.taskType == TaskType.SubTask && task == currentSubTask)
            {
                currentSubTask = null;
            }
        }
    }

    private QuestData FindTaskByID(TaskID id)
    {
        foreach (var task in mainTasks)
            if (task.taskID == id) return task;

        foreach (var task in subTasks)
            if (task.taskID == id) return task;

        return null;
    }

    private QuestData GetNextMainTask(TaskID current)
    {
        int index = (int)current;
        for (int i = index + 1; i <= (int)TaskID.MainTask10; i++)
        {
            QuestData next = mainTasks.Find(t => (int)t.taskID == i && !t.isCompleted);
            if (next != null) return next;
        }
        return null;
    }
}
