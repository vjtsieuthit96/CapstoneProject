using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    [SerializeField] public int currentSkillPoints;
    public SkillTreeSystem Sts;
    private void Start()
    {
        Sts = GetComponent<SkillTreeSystem>();
    }
    private void Update()
    {
        currentSkillPoints = Sts.availableSkillPoints;
    }
    private void OnEnable()
    {
        EventsManager.Instance.skillTreePointEvents.onSkillNodeUnlocked += HandleNodeUnlocked;
        EventsManager.Instance.skillTreePointEvents.onSkillPointAdded += ModifySkillPoints;
    }

    private void OnDisable()
    {
        EventsManager.Instance.skillTreePointEvents.onSkillNodeUnlocked -= HandleNodeUnlocked;
        EventsManager.Instance.skillTreePointEvents.onSkillPointAdded -= ModifySkillPoints;
    }

    private void HandleNodeUnlocked(SkillNode node)
    {
        if (node == null || node.isUnlocked)
            return;

        if (node.CanUnlock(currentSkillPoints))
        {
            currentSkillPoints -= node.requiredPoints;
            node.isUnlocked = true;

            var button = FindButtonForNode(node);
            if (button != null)
                button.Unlock();
        }
        else
        {
            Debug.Log("Không đủ điểm để mở node: " + node.displayName);
        }
    }

    private void ModifySkillPoints(int delta)
    {
        currentSkillPoints += delta;
    }

    private SkillNodeButton FindButtonForNode(SkillNode node)
    {
        foreach (var btn in FindObjectsByType<SkillNodeButton>(FindObjectsSortMode.None))
        {
            if (btn.name == node.name)
                return btn;
        }
        return null;
    }
}
