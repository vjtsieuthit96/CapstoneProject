using UnityEngine;
using UnityEngine.UI;

public class SkillNodeButton : MonoBehaviour
{
    public Image icon;
    public Button button;
    public GameObject lockedOverlay;
    public Text costText;

    private SkillNode node;
    private SkillTreeSystem system;

    public void Setup(SkillNode node, SkillTreeSystem system)
    {
        this.node = node;
        this.system = system;

        icon.sprite = node.icon;
        costText.text = node.requiredPoints.ToString();

        button.onClick.AddListener(() => {
            if (system.TryUnlock(node))
                UpdateVisual();
        });

        system.onSkillPointsChanged += UpdateVisual;
        UpdateVisual();
    }

    void OnDestroy()
    {
        system.onSkillPointsChanged -= UpdateVisual;
    }

    public void UpdateVisual()
    {
        bool canUnlock = node.CanUnlock() && system.GetPoints() >= node.requiredPoints;

        button.interactable = canUnlock && !node.isUnlocked;
        lockedOverlay.SetActive(!node.isUnlocked);

        icon.color = node.isUnlocked ? Color.white : Color.gray;
    }
}
