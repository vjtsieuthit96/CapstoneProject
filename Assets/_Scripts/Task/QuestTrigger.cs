using Invector.vCharacterController;
using System.Collections;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public QuestData questData;
    public Transform QuestTransform;
    public AudioSource audioSource;

    private bool triggered = false;
    [SerializeField] public CameraTargetSwitcher cameraSwitcher;

    public QuestData Taskcomplete;
    private void Awake()
    {
        if(!PlayerRealTimeData.Instance.isNewGame)
        {
            if (questData != null)
            {
                if (PlayerRealTimeData.Instance.completedMainTasks.Contains(questData.taskID))
                {
                    questData.isCompleted = true;
                    gameObject.SetActive(false);
                }
            }
            if(QuestManager.Instance.currentSubTask == questData)
            {
                this.gameObject.SetActive(false);
            }

        }
        else questData.isCompleted = false;
        if(QuestTransform != null)
        {
            QuestTransform.gameObject.SetActive(false);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || questData == null) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (QuestTransform != null)
        {
            QuestTransform.gameObject.SetActive(true);
            PathDrawer.Instance.SetTarget(QuestTransform);
        }
        var input = other.GetComponentInParent<vThirdPersonInput>();
        var control = other.GetComponentInParent<vThirdPersonController>();
        if (input == null) return;

        triggered = true;

        QuestManager.Instance.ReceiveQuest(questData);

        if (questData.taskType == TaskType.MainTask && QuestTransform != null)
        {

            PlayerRealTimeData.Instance.SetCheckpoint(transform.position, transform.rotation);
            CompleteLastTask(Taskcomplete);
            if (cameraSwitcher != null)
            {
                if (cameraSwitcher.targets.Count == 0)
                {
                    cameraSwitcher.targets.Add(QuestTransform);
                }
                else if (cameraSwitcher.targets.Count > 1)
                {
                    cameraSwitcher.targets.RemoveAt(1);
                }
                gameObject.SetActive(false);
            }

        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if(questData.isCompleted)
        {
            gameObject.SetActive(false);
        }    
    }

    public void CompleteLastTask(QuestData LastTask)
    {
        if(LastTask != null)
        {
            LastTask.isCompleted = true;
            PlayerRealTimeData.Instance.AddCompletedMainTask(LastTask.taskID);
        }
    }
}
