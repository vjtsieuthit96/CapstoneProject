using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SkillNode
{
    public string id;
    public string displayName;
    public string description;
    public Sprite icon;

    public List<SkillNode> prerequisites;
    public bool isUnlocked;

    [HideInInspector] public SkillEffect effect;

    public bool CanUnlock() =>
        !isUnlocked && (prerequisites == null || prerequisites.All(p => p.isUnlocked));

    public void Unlock()
    {
        if (CanUnlock())
        {
            isUnlocked = true;
            effect?.ApplyEffect();
        }
    }
}
