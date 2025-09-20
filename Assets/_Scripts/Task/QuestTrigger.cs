using UnityEngine;
using Invector.vCharacterController;
using System.Collections;

public class QuestTrigger : MonoBehaviour
{
    public QuestData questData;
    public Transform QuestTransform;
    public AudioSource audioSource;

    private bool triggered = false;
    [SerializeField] public CameraTargetSwitcher cameraSwitcher;

    private void Awake()
    {
        questData.isCompleted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || questData == null) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        var input = other.GetComponentInParent<vThirdPersonInput>();
        var control = other.GetComponentInParent<vThirdPersonController>();
        if (input == null) return;

        triggered = true;

        QuestManager.Instance.ReceiveQuest(questData);

        if (questData.taskType == TaskType.MainTask && QuestTransform != null)
        {
            PlayerRealTimeData.Instance.SetCheckpoint(transform.position, transform.rotation);
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
