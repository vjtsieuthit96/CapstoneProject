using UnityEngine;
using System;

public class SkillTreeSystem : MonoBehaviour
{
    public SkillTree skillTree;
    public int availableSkillPoints = 6;
    [SerializeField] private CharacterConfigurator characterConfigurator;

    public delegate void OnSkillPointsChanged();
    public event OnSkillPointsChanged onSkillPointsChanged;

    public bool TryUnlock(SkillNode node)
    {
        if (node.CanUnlock() && availableSkillPoints >= node.requiredPoints)
        {
            node.Unlock(characterConfigurator);
            availableSkillPoints -= node.requiredPoints;
            onSkillPointsChanged?.Invoke();
            return true;
        }
        return false;
    }

    public int GetPoints() => availableSkillPoints;
}
