using UnityEngine;
using Invector.vCharacterController;

public class SubQuestTrigger : QuestTrigger
{

    private void OnEnable()
    {
        base.OnEnable();

        if (questData != null)
        {
            questData.isCompleted = false;
        }

        triggered = false;

        if (QuestTransform != null)
        {
            QuestTransform.gameObject.SetActive(false);
        }
    }
}
