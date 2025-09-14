using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatsUIControl : MonoBehaviour
{
    [Range(0, 10)]
    public int activeCount = 0;

    private Image[] childImages;
    private int lastActiveCount = -1;

    public Color activeColor = new Color(0.7f, 0.85f, 1f);
    public Color defaultColor = Color.white;

    public float fillDuration = 0.6f;

    void Awake()
    {
        CacheChildImages();
    }

    void OnEnable()
    {
        CacheChildImages();
        lastActiveCount = -1;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void CacheChildImages()
    {
        childImages = GetComponentsInChildren<Image>(includeInactive: true);
        if (childImages.Length > 0 && childImages[0].gameObject == this.gameObject)
        {
            childImages = childImages.Skip(1).ToArray();
        }
    }

    void Update()
    {
        if (activeCount != lastActiveCount)
        {
            StopAllCoroutines();
            StartCoroutine(FillImagesGradually(activeCount));
            lastActiveCount = activeCount;
        }
    }

    IEnumerator FillImagesGradually(int targetCount)
    {
        if (childImages == null || childImages.Length == 0)
            yield break;

        for (int i = 0; i < childImages.Length; i++)
        {
            if (childImages[i] != null)
                childImages[i].color = defaultColor;
        }

        if (targetCount <= 0) yield break;

        float delay = fillDuration / targetCount;

        for (int i = 0; i < targetCount; i++)
        {
            if (i < childImages.Length && childImages[i] != null)
                childImages[i].color = activeColor;

            yield return new WaitForSeconds(delay);
        }
    }
}
