using UnityEngine;
using System;
using System.Collections.Generic;

public class SkillTreeSystem : MonoBehaviour
{
    public SkillTree skillTree;
    public int availableSkillPoints = 6;
    [SerializeField] private CharacterConfigurator characterConfigurator;

    public delegate void OnSkillPointsChanged();
    public event OnSkillPointsChanged onSkillPointsChanged;

    private void Start()
    {
        RefreshOnStart();
    }
    void Update()
    {
        foreach (var node in skillTree.allNodes)
        {
            if (node.isUnlocked && node.effect != null)
            {
                node.effect.UpdateCondition(characterConfigurator);
            }
        }
    }
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

    public void RefreshOnStart()
    {
        List<SkillNode> skillNode = skillTree.allNodes;
        foreach (SkillNode node in skillNode)
        {
            node.isUnlocked = false;
        }    
    }

    public int GetPoints() => availableSkillPoints;
}
