using UnityEngine;
using UnityEngine.UI;

public class SkillNodeButton : MonoBehaviour
{
    //public Image icon;
    public Button button;
    //public GameObject lockedOverlay;
    //public Text costText;

    [SerializeField] private SkillNode node;
    private void Start()
    {
        button = GetComponent<Button>();     
        button.onClick.AddListener(OnButtonClick);
    }
    //private SkillTreeSystem system;

    //public void Setup(SkillNode node, SkillTreeSystem system)
    //{
    //    this.node = node;
    //    this.system = system;

    //    icon.sprite = node.icon;
    //    costText.text = node.requiredPoints.ToString();

    //    button.onClick.AddListener(() => {
    //        if (system.TryUnlock(node))
    //            UpdateVisual();
    //    });

    //    system.onSkillPointsChanged += UpdateVisual;
    //    UpdateVisual();
    //}

    //void OnDestroy()
    //{
    //    system.onSkillPointsChanged -= UpdateVisual;
    //}

    //public void UpdateVisual()
    //{
    //    bool canUnlock = node.CanUnlock() && system.GetPoints() >= node.requiredPoints;

    //    button.interactable = canUnlock && !node.isUnlocked;

    //}

    public void UpdateUI(int point)
    {
        if (node == null) Debug.Log("Node doesn't exits");
        if(node.CanUnlock(point))
        {
            Debug.Log("Can unlock: " + node.displayName);
        }
        else
        {
            Debug.Log("Cannot unlock: " + node.displayName + " - Required Points: " + node.requiredPoints);
        }
    }

    public void Unlock()
    {
        Debug.Log("Unlocking: " + node.displayName);
        EventsManager.Instance.skillTreePointEvents.OnSkillPointAdded(-node.requiredPoints);

    }
    private void OnButtonClick()
    { 
        EventsManager.Instance.skillTreePointEvents.OnSkillNodeUnlocked(node);
        Debug.Log("Try To Unlock...");
    }
}
