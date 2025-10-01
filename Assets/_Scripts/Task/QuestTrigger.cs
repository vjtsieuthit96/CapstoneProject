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
    public GameObject NextMainTask;
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
        if(NextMainTask != null)
        {
            NextMainTask.SetActive(false);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || questData == null) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if(NextMainTask != null)
        {
            NextMainTask.SetActive(true);
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
            }

            input.SetLockAllInput(true);
            control.StopCharacter();

            StartCoroutine(WaitAndRestore(5f, input));
            StartCoroutine(SwitchToTarget(5f));
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

    private IEnumerator SwitchToTarget(float delay)
    {
        if (cameraSwitcher == null || QuestTransform == null) yield break;
        if (cameraSwitcher.currentIndex < 0 || cameraSwitcher.currentIndex >= cameraSwitcher.targets.Count) yield break;

        Transform previousTarget = cameraSwitcher.targets[cameraSwitcher.currentIndex];
        cameraSwitcher.SwitchTargetToTransform(QuestTransform);

        yield return new WaitForSeconds(delay);

        if (previousTarget != null)
            cameraSwitcher.SwitchTargetToTransform(previousTarget);
    }

    private IEnumerator WaitAndRestore(float delay, vThirdPersonInput input)
    {
        yield return new WaitForSeconds(delay);

        if (input != null)
            input.SetLockAllInput(false);

        gameObject.SetActive(false);
    }
}
