using UnityEngine;
using UnityEngine.UI;

public class SkillPointUI : MonoBehaviour
{
    public SkillTreeSystem skillSystem;
    public Text pointText;

    void Start()
    {
        skillSystem.onSkillPointsChanged += UpdateUI;
        UpdateUI();
    }

    void UpdateUI()
    {
        pointText.text = $"Skill Points: {skillSystem.GetPoints()}";
    }
}
