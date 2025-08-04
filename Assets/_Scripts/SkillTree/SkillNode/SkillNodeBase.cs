using UnityEngine;

[CreateAssetMenu(fileName = "AutoUnlockedSkillNode", menuName = "Scriptable Objects/SkillNode/AutoUnlocked")]
public class SkillNodeBase : SkillNode
{
    [Header("Auto Unlock Setup")]
    public SkillNode prerequisiteA;
    public SkillNode prerequisiteB;
    public SkillNode prerequisiteC;

    public void ApplyAutoUnlock()
    {
        if (prerequisites == null || prerequisites.Count == 0)
        {
            prerequisites = new System.Collections.Generic.List<SkillNode>();

            if (prerequisiteA != null) prerequisites.Add(prerequisiteA);
            if (prerequisiteB != null) prerequisites.Add(prerequisiteB);
            if (prerequisiteC != null) prerequisites.Add(prerequisiteC);
        }
        isUnlocked = true;
    }
}
