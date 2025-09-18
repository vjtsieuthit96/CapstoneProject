using Invector.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        // Khởi tạo reference lần đầu
        RefreshReferences();
        RefreshOnStart();
        ApplySkillTreeState();
        UpdateButtonsUI();

        foreach (var n in skillTree.allNodes)
        {
            n.TryAutoUnlock(characterConfigurator);
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

    // 🔹 Hàm tiện ích để tự động tìm lại reference khi null
    private void RefreshReferences()
    {
        if (characterConfigurator == null)
            characterConfigurator = FindObjectOfType<CharacterConfigurator>();

        if (offenceList == null)
            offenceList = FindListInfoByName("Offence");

        if (defenceList == null)
            defenceList = FindListInfoByName("Defence");

        if (vietnegryList == null)
            vietnegryList = FindListInfoByName("Vietnegy");
    }

    public SkillTreeListInfo FindListInfoByName(string objectName)
    {
        SkillTreeListInfo[] allLists = FindObjectsOfType<SkillTreeListInfo>(true);
        foreach (var list in allLists)
        {
            if (list.gameObject.name == objectName)
                return list;
        }
        return null;
    }

    public void OnSkillPointAdded(int skillPoint)
    {
        availableSkillPoints += skillPoint;
        UpdateButtonsUI();
    }

    private void OnSkillNodeUnlocked(SkillNode node)
    {
        if (node.CanUnlock(availableSkillPoints))
        {
            ApplySkillTreeState();
        }
        else
        {
            Debug.Log("Cannot unlock: " + node.displayName + " - Required Points: " + node.requiredPoints);
            return;
        }

        UpdateButtonsUI();
    }

    private void Update()
    {
        // 🔹 Check null mỗi frame -> nếu respawn mất reference thì gán lại
        RefreshReferences();

        if (skillTree != null && characterConfigurator != null)
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

    public bool TryUnlock(SkillNode node)
    {
        if (node.CanUnlock(availableSkillPoints) && availableSkillPoints >= node.requiredPoints)
        {
            availableSkillPoints -= node.requiredPoints;
            onSkillPointsChanged?.Invoke();
            return true;
        }
        return false;
    }

    private void UpdateButtonsUI()
    {
        if (offenceList != null && offenceList.skillNodeButtons.Length > 0)
        {
            foreach (var btn in offenceList.skillNodeButtons)
                btn.UpdateUI(availableSkillPoints);
        }

        if (defenceList != null && defenceList.skillNodeButtons.Length > 0)
        {
            foreach (var btn in defenceList.skillNodeButtons)
                btn.UpdateUI(availableSkillPoints);
        }

        if (vietnegryList != null && vietnegryList.skillNodeButtons.Length > 0)
        {
            foreach (var btn in vietnegryList.skillNodeButtons)
                btn.UpdateUI(availableSkillPoints);
        }
    }

    public void RefreshOnStart()
    {
        if (skillTree == null) return;

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
