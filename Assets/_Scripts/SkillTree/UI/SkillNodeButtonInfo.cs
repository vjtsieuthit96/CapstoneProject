using UnityEngine;
using UnityEngine.EventSystems;

public class SkillNodeButtonInfo : MonoBehaviour ,IPointerEnterHandler
{
    private SkillNodeButton skillNodeButton;
    [SerializeField] ScrollingText text;

    public void OnPointerEnter(PointerEventData eventData)
    {
       if(skillNodeButton == null )
        {
            Debug.LogWarning("SkillNodeButton component is not assigned in SkillNodeButtonInfo.");
            return;
        }
        if (text)
        {
            string information = skillNodeButton.NodeInfo();
            text.ActivateText(information);
        }
    }

    private void Start()
    {
        skillNodeButton = GetComponent<SkillNodeButton>();
    }

}
