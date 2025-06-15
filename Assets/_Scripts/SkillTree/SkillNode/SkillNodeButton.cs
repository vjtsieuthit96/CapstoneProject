using UnityEngine;
using UnityEngine.UI;

public class SkillNodeButton : MonoBehaviour
{
    public Image icon;
    public Button button;
    public GameObject lockedOverlay;

    private SkillNode node;
    private SkillTreeSystem system;

    public void Setup(SkillNode node, SkillTreeSystem system)
    {
        this.node = node;
        this.system = system;
        icon.sprite = node.icon;

        button.onClick.AddListener(() => system.TryUnlock(node));
        UpdateVisual();
    }

    void Update()
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        button.interactable = node.CanUnlock();
        lockedOverlay.SetActive(!node.isUnlocked);
        icon.color = node.isUnlocked ? Color.white : Color.gray;
    }
}
