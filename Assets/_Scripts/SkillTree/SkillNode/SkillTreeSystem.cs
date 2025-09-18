using Invector.Utils;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

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
    private float saveTimer;

    private void Start()
    {
        characterConfigurator = FindObjectOfType<CharacterConfigurator>();
        offenceList = FindListInfoByName("Offence");
        defenceList = FindListInfoByName("Defence");
        vietnegryList = FindListInfoByName("Vietnegy");
        RefreshOnStart();
        ApplySkillTreeState();
        UpdateButtonsUI();
        foreach(var n in skillTree.allNodes)
        {
            n.TryAutoUnlock(characterConfigurator);
        }    

    }
    public SkillTreeListInfo FindListInfoByName(string objectName)
    {
        SkillTreeListInfo[] allLists = FindObjectsOfType<SkillTreeListInfo>(true);
        foreach (var list in allLists)
        {
            if (list.gameObject.name == objectName)
            {
                return list;
            }
        }
        return null;
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
            ApplySkillTreeState();
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
    private void LateUpdate()
    {
        saveTimer += Time.deltaTime;
        if (saveTimer >= 3f)
        {
            saveTimer = 0f;
            CaptureSkillTreeState();
        }
    }

    // neu thoa dieu kien, tu dong tra cong ki nang ?
    public bool TryUnlock(SkillNode node)
    {
        if (node.CanUnlock(availableSkillPoints) && availableSkillPoints >= node.requiredPoints)
        {
            //node.Unlock(characterConfigurator);
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

    private void CaptureSkillTreeState()
    {
        if (skillTree == null || PlayerRealTimeData.Instance == null) return;

        PlayerRealTimeData.Instance.currentSkillTreeState.nodeStates.Clear();

        foreach (var node in skillTree.allNodes)
        {
            SkillNodeState state = new SkillNodeState
            {
                nodeId = node.id,
                isUnlocked = node.isUnlocked
            };
            PlayerRealTimeData.Instance.currentSkillTreeState.nodeStates.Add(state);
        }

    }
    public void ApplySkillTreeState()
    {
        if (skillTree == null || PlayerRealTimeData.Instance == null) return;

        var savedStates = PlayerRealTimeData.Instance.currentSkillTreeState.nodeStates;

        foreach (var node in skillTree.allNodes)
        {
            var saved = savedStates.Find(s => s.nodeId == node.id);
            if (saved != null)
            {
                node.isUnlocked = saved.isUnlocked;
            }
        }

        UpdateButtonsUI();
    }

    public int GetPoints() => availableSkillPoints;

}
