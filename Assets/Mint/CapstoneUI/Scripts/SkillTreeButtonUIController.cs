using UnityEngine;

public class SkillTreeButtonUIController : MonoBehaviour
{
    [SerializeField] private RectTransform offenceRectTransform;
    private RectTransform currentOffenceRT;
    [SerializeField] private RectTransform defenceRectTransform;
    private RectTransform currentDefenceRT;
    [SerializeField] private RectTransform vietnegyRectTransform;
    private RectTransform currentVietnegyRT;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentOffenceRT = offenceRectTransform;
        currentDefenceRT = defenceRectTransform;
        currentVietnegyRT = vietnegyRectTransform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
