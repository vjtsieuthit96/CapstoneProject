using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewSkillNode", menuName = "Scriptable Objects/SkillNode")]
public class SkillNode : ScriptableObject
{
    public string id;
    public string displayName;
    public string description;
    public Image icon;

    public List<SkillNode> prerequisites;
    public UnlockConditionType unlockCondition = UnlockConditionType.AllPrerequisites;

    public bool isUnlocked;
    public int requiredPoints = 1;

    public SkillEffect effect;

    public bool CanUnlock()
    {
        if (isUnlocked) return false;
        if (prerequisites == null || prerequisites.Count == 0) return true;

        switch (unlockCondition)
        {
            case UnlockConditionType.AllPrerequisites:
                return prerequisites.All(p => p.isUnlocked);

            case UnlockConditionType.AnyPrerequisite:
                return prerequisites.Any(p => p.isUnlocked);
        }

        return false;
    }

    public void Unlock(CharacterConfigurator configurator)
    {
        isUnlocked = true;
        effect?.ApplyEffect(configurator);
    }
}

public enum UnlockConditionType
{
    AllPrerequisites,
    AnyPrerequisite
}
