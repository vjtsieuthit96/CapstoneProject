using UnityEngine;
using Invector.vCharacterController;
using System.Collections;
public class QuestTrigger : MonoBehaviour
{
    public QuestData questData;
    public AudioSource audioSource;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || questData == null) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        var input = other.GetComponentInParent<vThirdPersonInput>();
        var Control = other.GetComponentInParent<vThirdPersonController>();
        if (input == null) return;

        triggered = true;
        input.SetLockAllInput(true);
        Control.StopCharacter();
        audioSource.clip = questData.questAudio;
        audioSource.Play();

        QuestManager.Instance.ReceiveQuest(questData);

        StartCoroutine(WaitAndUnlock(audioSource.clip.length, input));
    }


    private IEnumerator WaitAndUnlock(float delay, vThirdPersonInput input)
    {
        yield return new WaitForSeconds(delay);
        input.SetLockAllInput(false);
        this.gameObject.SetActive(false);
    }
}
