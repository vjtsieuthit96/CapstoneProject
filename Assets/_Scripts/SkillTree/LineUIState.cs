using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineUIState : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Các Image dạng Line để fill")]
    [SerializeField] private List<Image> lineImages = new();

    [Header("Animation Settings")]
    [Tooltip("Thời gian fill từ 0 -> 1 (giây)")]
    [SerializeField] private float fillDuration = 0.5f;

    [Tooltip("Tự động tìm SkillNodeButton ở cha?")]
    [SerializeField] private bool autoFindButton = true;

    private SkillNodeButton skillNodeButton;
    [SerializeField] private bool isFilling = false;
    [SerializeField] private bool hasFinishedFill = false;

    private void Awake()
    {
        hasFinishedFill = false;
        isFilling = false;
        ResetLines();
        if (autoFindButton)
            skillNodeButton = GetComponentInParent<SkillNodeButton>();
    }

    private void OnEnable()
    {
        ResetLines();
        if (skillNodeButton != null && skillNodeButton.unlockedState)
        {
            hasFinishedFill = false;
            StartCoroutine(AnimateFill());
        }
    }

    private void Update()
    {
        if (!hasFinishedFill && !isFilling && skillNodeButton != null && skillNodeButton.unlockedState)
        {
            StartCoroutine(AnimateFill());
        }
    }

    private void ResetLines()
    {
        hasFinishedFill = false;
        foreach (var line in lineImages)
        {
            if (line != null)
                line.fillAmount = 0f;
        }
    }

    private IEnumerator AnimateFill()
    {
        isFilling = true;
        float elapsed = 0f;

        while (elapsed < fillDuration)
        {
            float t = elapsed / fillDuration;
            foreach (var line in lineImages)
            {
                if (line != null)
                    line.fillAmount = Mathf.Lerp(0f, 1f, t);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var line in lineImages)
        {
            if (line != null)
                line.fillAmount = 1f;
        }

        isFilling = false;
        hasFinishedFill = true;
    }
}
