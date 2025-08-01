using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Invector.Utils;

public class SkillTreeSystem : MonoBehaviour
{
    public SkillTree skillTree;
    public int availableSkillPoints = 6;
    [SerializeField] private CharacterConfigurator characterConfigurator;
    [Header("ButtonLists")]

    [SerializeField] private SkillTreeListInfo offenceList;
    [SerializeField] private SkillTreeListInfo defenceList; 
    [SerializeField] private SkillTreeListInfo vietnegryList;

    public delegate void OnSkillPointsChanged();
    public event OnSkillPointsChanged onSkillPointsChanged;

    private void Start()
    {
        RefreshOnStart();
        UpdateButtonsUI();
       
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
        UpdateButtonsUI();
        //onSkillPointsChanged?.Invoke();
    }
    // mo khoa ki nang
    private void OnSkillNodeUnlocked(SkillNode node)
    {
        if (node.CanUnlock(availableSkillPoints))
        {
            node.Unlock(characterConfigurator);
            availableSkillPoints -= node.requiredPoints;
            Debug.Log("Unlocked: " + node.displayName + " - Remaining Points: " + availableSkillPoints);
        }
        else { Debug.Log("Cannot unlock: " + node.displayName + " - Required Points: " + node.requiredPoints); return; }

        UpdateButtonsUI();
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
    private void UpdateButtonsUI()
    {
        if(offenceList.skillNodeButtons.Length > 0)
        {
            foreach (var btn in offenceList.skillNodeButtons)
            {
                btn.UpdateUI(availableSkillPoints);
            }
        }
       
        if(defenceList.skillNodeButtons.Length > 0)
        {
            foreach (var btn in defenceList.skillNodeButtons)
            {
                btn.UpdateUI(availableSkillPoints);
            }
        }
        if(vietnegryList.skillNodeButtons.Length > 0)
        {
            foreach (var btn in vietnegryList.skillNodeButtons)
            {
                btn.UpdateUI(availableSkillPoints);
            }
        }
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
