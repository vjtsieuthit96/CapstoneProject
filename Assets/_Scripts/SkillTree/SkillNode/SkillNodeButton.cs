using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SkillNodeButtonInfo))]
public class SkillNodeButton : MonoBehaviour
{
    //public Image icon;
    private Button button;
    [SerializeField] private Image unlockedImg;
    //public GameObject lockedOverlay;
    //public Text costText;

    [SerializeField] private SkillNode node;
    private void Start()
    {
        button = GetComponent<Button>();
        unlockedImg = GetComponentInChildren<Image>();
        button.onClick.AddListener(OnButtonClick);
    }

    public bool IsUnlocked
    {
        get { return node.isUnlocked; }
    }

    public void UpdateUI(int point)
    {
        if (node == null) Debug.Log("Node doesn't exits");
        if(node.isUnlocked)
        {
            unlockedImg.color = Color.blue;
            return;
        }

        if (node.CanUnlock(point))
        {
            unlockedImg.color = Color.cyan;
        }
        else
        {
            unlockedImg.color = Color.black;
            Debug.Log("Cannot unlock: " + node.displayName + " - Required Points: " + node.requiredPoints);
        }
    }

    public void Unlock()
    {
        Debug.Log("Unlocking: " + node.displayName);
        EventsManager.Instance.skillTreePointEvents.OnSkillPointAdded(-node.requiredPoints);
        unlockedImg.color = Color.blue;

    }
   
    private void OnButtonClick()
    { 
        EventsManager.Instance.skillTreePointEvents.OnSkillNodeUnlocked(node);
        Debug.Log("Try To Unlock...");
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

        
        if(!node.isUnlocked)
        {
            sb.AppendLine($"Required Points: {node.requiredPoints}");
        }

        return sb.ToString();
    }
}
