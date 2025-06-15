using UnityEngine;

public class SkillTreeUIManager : MonoBehaviour
{
    public SkillTreeSystem skillSystem;
    public GameObject skillNodeButtonPrefab;
    public Transform nodeContainer;

    void Start()
    {
        foreach (var node in skillSystem.skillTree.allNodes)
        {
            var go = Instantiate(skillNodeButtonPrefab, nodeContainer);
            var button = go.GetComponent<SkillNodeButton>();
            button.Setup(node, skillSystem);
        }
    }
}
