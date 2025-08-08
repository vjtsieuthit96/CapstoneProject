using UnityEngine;

public class ScrollingTextTimeLine : MonoBehaviour
{
    [TextArea]
    public string timelineText;

    public ScrollingText scrollingText;

    public void Play()
    {
        scrollingText.ActivateText(timelineText);
    }    
}
