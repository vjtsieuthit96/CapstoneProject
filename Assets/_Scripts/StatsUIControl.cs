using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatsUIControl : MonoBehaviour
{
    [Range(0, 10)]
    public int activeCount = 0;

    private Image[] childImages;

    public Color activeColor = new Color(0.7f, 0.85f, 1f);
    public Color defaultColor = Color.white;

    void Awake()
    {
        childImages = GetComponentsInChildren<Image>();
        if (childImages.Length > 0 && childImages[0].gameObject == this.gameObject)
        {
            childImages = childImages.Skip(1).ToArray();
        }
    }

    void Update()
    {
        UpdateImages();
    }

    void UpdateImages()
    {
        if (childImages == null) return;

        for (int i = 0; i < childImages.Length; i++)
        {
            if (i < activeCount)
            {
                childImages[i].color = activeColor;
            }
            else
            {
                childImages[i].color = defaultColor;
            }
        }
    }
}
