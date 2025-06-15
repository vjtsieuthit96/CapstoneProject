using UnityEngine;
using System.Collections.Generic;

public class SkillTreeUIManager : MonoBehaviour
{
    public SkillTreeSystem skillSystem;
    public GameObject skillNodeButtonPrefab;
    public Transform nodeContainer;

    private List<SkillNodeButton> buttons = new List<SkillNodeButton>();

    void Start()
    {
        foreach (var node in skillSystem.skillTree.allNodes)
        {
            var go = Instantiate(skillNodeButtonPrefab, nodeContainer);
            var button = go.GetComponent<SkillNodeButton>();
            button.Setup(node, skillSystem);
            buttons.Add(button);
        }
        skillSystem.onSkillPointsChanged += RefreshAll;
    }

    void RefreshAll()
    {
        foreach (var btn in buttons)
        {
            btn.UpdateVisual();
        }
    }
}
