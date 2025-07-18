using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillTreeSystem : MonoBehaviour
{
    public SkillTree skillTree;
    public int availableSkillPoints = 6;
    [SerializeField] private CharacterConfigurator characterConfigurator;
    [SerializeField] private SkillNodeButton[] OffenceBtn;

    public delegate void OnSkillPointsChanged();
    public event OnSkillPointsChanged onSkillPointsChanged;

    private void Start()
    {
        RefreshOnStart();
        foreach (var btn in OffenceBtn)
        {
            btn.UpdateUI(availableSkillPoints);
        }
    }
    private void OnEnable()
    {
        EventsManager.Instance.skillTreePointEvents.onSkillPointAdded += OnSkillPointAdded;
        EventsManager.Instance.skillTreePointEvents.onSkillNodeUnlocked += OnSkillNodeUnlocked;
    }
    private void OnDisable()
    {
        EventsManager.Instance.skillTreePointEvents.onSkillPointAdded -= OnSkillPointAdded;
        EventsManager.Instance.skillTreePointEvents.onSkillNodeUnlocked -= OnSkillNodeUnlocked;
    }

    public void OnSkillPointAdded(int skillPoint)
    {
        availableSkillPoints += skillPoint;
        foreach (var btn in OffenceBtn)
        {
           btn.UpdateUI(availableSkillPoints);
        }
        //onSkillPointsChanged?.Invoke();
    }
    private void OnSkillNodeUnlocked(SkillNode node)
    {
        if (node.CanUnlock(availableSkillPoints))
        {
            node.Unlock(characterConfigurator);
            availableSkillPoints -= node.requiredPoints;
            Debug.Log("Unlocked: " + node.displayName + " - Remaining Points: " + availableSkillPoints);
        }
        else { Debug.Log("Cannot unlock: " + node.displayName + " - Required Points: " + node.requiredPoints); return; }

        foreach (var btn in OffenceBtn)
            {
                btn.UpdateUI(availableSkillPoints);
            }
    }
    void Update()
    {
        if(skillTree!= null)
        {
            foreach (var node in skillTree.allNodes)
            {
                if (node.isUnlocked && node.effect != null)
                {
                    node.effect.UpdateCondition(characterConfigurator);
                }
            }
        }
       
    }
    // neu thoa dieu kien, tu dong tra cong ki nang ?
    public bool TryUnlock(SkillNode node)
    {
        if (node.CanUnlock(availableSkillPoints) && availableSkillPoints >= node.requiredPoints)
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
