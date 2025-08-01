using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreeListInfo : MonoBehaviour, IPointerEnterHandler
{
    public SkillNodeButton[] skillNodeButtons;

    [SerializeField] private ScrollingText text;

    private int totalPointsActived = 0;
    public void OnPointerEnter(PointerEventData eventData)
    {
        totalPointsActived = 0;
        foreach (var button in skillNodeButtons)
        {
            if (button.IsUnlocked)
            {
                totalPointsActived++;
            }
        }
        string information = $"{this.name}\npoint actived: {totalPointsActived.ToString()}";
        if (text != null)
        {
            text.ActivateText(information);
        }
        else
        {
            Debug.LogWarning("ScrollingText component is not assigned in SkillTreeListInfo.");
        }
    }
}