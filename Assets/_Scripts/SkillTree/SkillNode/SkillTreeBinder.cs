using UnityEngine;
using System.Collections.Generic;

public class SkillTreeBinder : MonoBehaviour
{
    public SkillTreeSystem skillSystem;

    [System.Serializable]
    public class NodeBinding
    {
        public SkillNodeButton button;
        public SkillNode node;
    }

    public List<NodeBinding> bindings;

    void Start()
    {
        foreach (var binding in bindings)
        {
            binding.button.Setup(binding.node, skillSystem);
        }

        skillSystem.onSkillPointsChanged += RefreshAll;
    }

    void RefreshAll()
    {
        foreach (var binding in bindings)
        {
            binding.button.UpdateVisual();
        }
    }
}
