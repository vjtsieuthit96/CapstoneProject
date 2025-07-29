using UnityEngine; 
using System.Collections.Generic;
using Invector.Utils;
using System;
using Unity.VisualScripting.ReorderableList;
using System.Collections;

public class SkillTreeUIManager : MonoBehaviour
{
    // OffenceButton
    [SerializeField] private vFadeCanvas offenceBtnCanvas;
    [SerializeField] private vFadeCanvas defenceBtnCanvas;
    [SerializeField] private vFadeCanvas vietnegyfadeCanvas;
    public SkillTreeSystem skillSystem;
    public Transform nodeContainer;

    private RectTransform rectTransform;
    private RectTransform rectTransformDefence;
    private RectTransform rectShieldVietnegry;
    private List<SkillNodeButton> buttons = new List<SkillNodeButton>();

    private void Start()
    {
        rectTransform = offenceBtnCanvas.GetComponent<RectTransform>();
        rectTransformDefence = defenceBtnCanvas.GetComponent<RectTransform>();
        rectShieldVietnegry = vietnegyfadeCanvas.GetComponent<RectTransform>();

    }
    private void OnEnable()
    {
        offenceBtnCanvas.AlphaFull();
        defenceBtnCanvas.AlphaFull();
        vietnegyfadeCanvas.AlphaFull();
       
    }
    private void OnDisable()
    {
        var rect = defenceBtnCanvas.GetComponent<RectTransform>();
        rect.anchoredPosition = rectTransformDefence.anchoredPosition;
        var rectVietnegy = vietnegyfadeCanvas.GetComponent<RectTransform>();
        rectVietnegy.anchoredPosition = rectShieldVietnegry.anchoredPosition;
    }
    public void OpenOffencePanel()
    {
        FadeIn(0);
    }
    public void OpenDefencePanel()
    {
        FadeIn(1);
    }
    public void OpenVietnegyPanel()
    {
        FadeIn(2);
    }

    private void FadeIn(int index)
    {

        switch (index)
        {
            case 0:
                offenceBtnCanvas.FadeIn();
                defenceBtnCanvas.FadeOut();
                vietnegyfadeCanvas.FadeOut();
                break;
            case 1:
                defenceBtnCanvas.FadeIn();
                offenceBtnCanvas.FadeOut();
                vietnegyfadeCanvas.FadeOut();
                StartCoroutine(ButtonMovingOn(1));
                break;
            case 2:
                vietnegyfadeCanvas.FadeIn();
                offenceBtnCanvas.FadeOut();
                defenceBtnCanvas.FadeOut();
                StartCoroutine(ButtonMovingOn(2));
                break;
            default:
                Debug.LogWarning("Invalid index for FadeIn");
                break;
        }

    }


    private IEnumerator ButtonMovingOn(int index)
    {
        yield return new WaitForSeconds(0.5f);
        if (index == 1)
        {
          
            RectTransform rect = defenceBtnCanvas.GetComponent<RectTransform>();
            while (Vector2.Distance(rect.anchoredPosition, rectTransformDefence.anchoredPosition) > 0.1f)
            {
                Debug.Log("Moving... " + rect.anchoredPosition + " to " + rectTransformDefence.anchoredPosition);
                rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, rectTransformDefence.anchoredPosition, 0.1f);
                yield return null;
            }
          
        }else
        if (index == 2)
        {
            RectTransform rect = vietnegyfadeCanvas.GetComponent<RectTransform>();
            while (Vector2.Distance(rect.anchoredPosition, rectShieldVietnegry.anchoredPosition) > 0.1f)
            {
                rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, rectShieldVietnegry.anchoredPosition, 0.1f);
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("Invalid index for ButtonMovingOn");
        }
    }
}
