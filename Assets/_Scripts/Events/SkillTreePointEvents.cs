using UnityEngine;
using System;
public class SkillTreePointEvents : MonoBehaviour
{
    public event Action<int> onSkillPointAdded;
    public void OnSkillPointAdded(int skillPoint)
    {
        if(onSkillPointAdded != null)
            onSkillPointAdded(skillPoint);
    }

    public event Action<SkillNode> onSkillNodeUnlocked;
    public void OnSkillNodeUnlocked(SkillNode node)
    {
        if (onSkillNodeUnlocked != null)
            onSkillNodeUnlocked(node);
    }

}
