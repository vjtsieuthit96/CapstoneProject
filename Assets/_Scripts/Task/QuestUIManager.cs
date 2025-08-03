using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager Instance { get; private set; }

    [Header("Main Task Panel")]
    [SerializeField] private GameObject mainTaskPanel;
    [SerializeField] private Text mainTaskTitleText;
    [SerializeField] private Text mainTaskDescText;

    [Header("Sub Task Panel")]
    [SerializeField] private GameObject subTaskPanel;
    [SerializeField] private Text subTaskTitleText;
    [SerializeField] private Text subTaskDescText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        mainTaskPanel.SetActive(false);
        subTaskPanel.SetActive(false);
    }

    public void ShowTask(QuestData quest)
    {
        if (quest.taskType == TaskType.MainTask)
        {
            mainTaskTitleText.text = quest.questName;
            mainTaskDescText.text = quest.description;
            mainTaskPanel.SetActive(true);
        }
        else
        {
            subTaskTitleText.text = quest.questName;
            subTaskDescText.text = quest.description;
            subTaskPanel.SetActive(true);
        }
    }

    public void HideTask(QuestData quest)
    {
        if (quest.taskType == TaskType.MainTask)
        {
            if (mainTaskTitleText.text == quest.questName)
            {
                mainTaskPanel.SetActive(false);
                mainTaskTitleText.text = "";
                mainTaskDescText.text = "";
            }
        }
        else
        {
            if (subTaskTitleText.text == quest.questName)
            {
                subTaskPanel.SetActive(false);
                subTaskTitleText.text = "";
                subTaskDescText.text = "";
            }
        }
    }
}
