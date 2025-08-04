using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(SkillNodeButtonInfo))]
public class SkillNodeButton : MonoBehaviour
{
    //public Image icon;
    [SerializeField] private Button button;
    [SerializeField] private Image unlockedImg;
    [SerializeField] public bool unlockedState;
    [SerializeField] public SkillTreeManager manager;
    [SerializeField] private CharacterConfigurator cc;

    [SerializeField] private SkillNode node;

    private void Start()
    {
        unlockedState = node.isUnlocked;
        button = GetComponent<Button>();
        unlockedImg = GetComponentInChildren<Image>();
        button.onClick.AddListener(() =>
        {
            OnButtonClick();
        });
    }
    private void Update()
    {
        CheckUI();
        if (unlockedState != node.isUnlocked)
        {
            unlockedState = node.isUnlocked;
        }
    }
    public bool IsUnlocked
    {
        get { return node.isUnlocked; }
    }

    void CheckUI()
    {
        if (unlockedState) return;
        else
        {
            UpdateUI(manager.currentSkillPoints);
        }

    }

    public void UpdateUI(int point)
    {
        if (node == null)
        {
            return;
        }

        if (node.isUnlocked)
        {
            unlockedImg.color = Color.white;
            button.interactable = true;
        }
        else if (node.CanUnlock(point))
        {
            unlockedImg.color = new Color(0.25f, 0.25f, 0.25f);
            button.interactable = true;
        }
        else
        {
            unlockedImg.color = Color.black;
            button.interactable = false;
        }

    }




    public void Unlock()
    {
        Debug.Log("Unlocking: " + node.displayName);
        EventsManager.Instance.skillTreePointEvents.OnSkillPointAdded(-node.requiredPoints);
        node.Unlock(cc);
        node.effect.ApplyEffect(cc);
        unlockedImg.color = Color.blue;

    }

    private void OnButtonClick()
    {
        Debug.Log("Try to unlock: " + node.displayName);
        if (node.CanUnlock(manager.Sts.availableSkillPoints))
        {
            EventsManager.Instance.skillTreePointEvents.OnSkillNodeUnlocked(node);
            manager.Sts.availableSkillPoints -= node.requiredPoints;
            Unlock();
        }
        else
        {
            Debug.Log("Không thể unlock node: " + node.displayName);
        }
    }

    public string NodeInfo()
    {
        if (node == null)
            return string.Empty;

        var sb = new System.Text.StringBuilder();
        if (node.isUnlocked)
        {
            sb.AppendLine("       <color=green>Activated</color>");
        }
        sb.AppendLine(node.displayName);
        sb.AppendLine(node.description);
        sb.AppendLine();


        if (!node.isUnlocked)
        {
            sb.AppendLine($"Required Points: {node.requiredPoints}");
        }

        return sb.ToString();
    }
    private void OnValidate()
    {
        if (node != null)
            unlockedState = node.isUnlocked;
    }
}
