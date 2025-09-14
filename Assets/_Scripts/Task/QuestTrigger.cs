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
    private Transform previousTarget;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || questData == null) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (cameraSwitcher.targets.Count == 0)
        {
            cameraSwitcher.targets.Add(QuestTransform);
        }
        else if(cameraSwitcher.targets.Count > 0)
        {
            cameraSwitcher.targets.RemoveAt(1);
        }    
            var input = other.GetComponentInParent<vThirdPersonInput>();
        var control = other.GetComponentInParent<vThirdPersonController>();
        if (input == null) return;

        triggered = true;
        input.SetLockAllInput(true);
        control.StopCharacter();
        QuestManager.Instance.ReceiveQuest(questData);
        StartCoroutine(WaitAndRestore(5f, input));
        StartCoroutine(SwitchToTarget(5f));
    }


    private IEnumerator SwitchToTarget(float delay)
    {
        Transform previousTarget = cameraSwitcher.targets[cameraSwitcher.currentIndex];
        cameraSwitcher.SwitchTargetToTransform(QuestTransform);
        yield return new WaitForSeconds(delay);
        cameraSwitcher.SwitchTargetToTransform(previousTarget);
    }

    private IEnumerator WaitAndRestore(float delay, vThirdPersonInput input)
    {
        yield return new WaitForSeconds(delay);
        input.SetLockAllInput(false);
        gameObject.SetActive(false);
    }
}
