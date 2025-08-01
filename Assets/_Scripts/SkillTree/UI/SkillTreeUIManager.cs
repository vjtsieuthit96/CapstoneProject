using UnityEngine;
using System.Collections.Generic;
using Invector.Utils;
using System;
using Unity.VisualScripting.ReorderableList;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;

public class SkillTreeUIManager : MonoBehaviour
{
    private UIPanel uiPanel;
    [Header("Option Board Controller")]
    [SerializeField] private OptionBoardController optionBoardController;
    [SerializeField] private TextMeshProUGUI totalPointText;
    private int totalPoint = 0;
    // OffenceButton
    [Header("ButtonCanvas")]
    [SerializeField] private vFadeCanvas offenceBtnCanvas;
    [SerializeField] private vFadeCanvas defenceBtnCanvas;
    [SerializeField] private vFadeCanvas vietnegyfadeCanvas;
    [Header("PanelCanvas")]
    [SerializeField] private vFadeCanvas offencePanelCanvas;
    [SerializeField] private vFadeCanvas defencePanelCanvas;
    [SerializeField] private vFadeCanvas vietnegyPanelCanvas;
    public SkillTreeSystem skillSystem;
    public Transform nodeContainer;
    [Header("Selected")]
    [SerializeField] private GameObject offenceSelected;
    [SerializeField] private GameObject defenceSelected;
    [SerializeField] private GameObject vietnegySelected;

    [SerializeField] private RectTransform rectTransformPanelOnPos;
    private Vector2 rectTransformOffencePos;
    private Vector2 rectTransformDefencePos;
    private Vector2 rectShieldVietnegryPos;
    private List<SkillNodeButton> buttons = new List<SkillNodeButton>();

    private bool isButtonShow = true;
    private void Start()
    {
        uiPanel = GetComponent<UIPanel>();

        rectTransformOffencePos = offenceBtnCanvas.GetComponent<RectTransform>().anchoredPosition;
        rectTransformDefencePos = defenceBtnCanvas.GetComponent<RectTransform>().anchoredPosition;
        rectShieldVietnegryPos = vietnegyfadeCanvas.GetComponent<RectTransform>().anchoredPosition;

        totalPoint = skillSystem.availableSkillPoints;
        totalPointText.text = "Skill Points: " + totalPoint.ToString();
        Restart();

    }
    private void OnEnable()
    {
        EventsManager.Instance.pressEvents.onOpitonButtonPress += OptionButtonActing;
        Restart();

    }
    private void OnDisable()
    {
        EventsManager.Instance.pressEvents.onOpitonButtonPress -= OptionButtonActing;

        var rectTransformOffence = offenceBtnCanvas.GetComponent<RectTransform>();
        rectTransformOffence.anchoredPosition = rectTransformOffencePos;

        var rectDefence = defenceBtnCanvas.GetComponent<RectTransform>();
        rectDefence.anchoredPosition = rectTransformDefencePos;

        var rectVietnegy = vietnegyfadeCanvas.GetComponent<RectTransform>();
        rectVietnegy.anchoredPosition = rectShieldVietnegryPos;
    }

    private void Update()
    {
        if (skillSystem.availableSkillPoints != totalPoint)
        {
            totalPoint = skillSystem.availableSkillPoints;
            totalPointText.text = "Skill Points: " + totalPoint.ToString();
        }
    }

    private void OptionButtonActing(PanelType type)
    {
        if (type == uiPanel.panelType && optionBoardController.isPanelChildActing)
        {
            //To Do out chil, return state;
            if (offenceSelected.activeSelf)
            {
                StartCoroutine(ButtonMoving(0, false));
            }
            else
            if (defenceSelected.activeSelf)
            {
                StartCoroutine(ButtonMoving(1, false));
            }
            else
            if (vietnegySelected.activeSelf)
            { StartCoroutine(ButtonMoving(2, false)); }
        }
        else
        if (type == uiPanel.panelType && !optionBoardController.isPanelChildActing)
        {
            optionBoardController.FadeIn(PanelType.Option);
        }
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
        if (!isButtonShow)
        {
            return;
        }
        else
            isButtonShow = false;
        optionBoardController.isPanelChildActing = true;
        switch (index)
        {
            case 0:
                offenceBtnCanvas.FadeIn();
                defenceBtnCanvas.FadeOut();
                vietnegyfadeCanvas.FadeOut();

                offenceSelected.SetActive(true);
                ButtonMovingOn(0);
                break;
            case 1:
                defenceBtnCanvas.FadeIn();
                offenceBtnCanvas.FadeOut();
                vietnegyfadeCanvas.FadeOut();

                defenceSelected.SetActive(true);
                ButtonMovingOn(1);
                break;
            case 2:
                vietnegyfadeCanvas.FadeIn();
                offenceBtnCanvas.FadeOut();
                defenceBtnCanvas.FadeOut();

                vietnegySelected.SetActive(true);
                ButtonMovingOn(2);
                break;
            default:
                Debug.LogWarning("Invalid index for FadeIn");
                break;
        }

    }

    public void ButtonMovingOn(int index)
    {
        StartCoroutine(ButtonMoving(index, true));
    }
    private IEnumerator ButtonMoving(int index, bool isTurnOn)
    {
        // TO DO: Turn off correct panel
        if (!isTurnOn)
        {
            if (offenceSelected.activeSelf)
                offenceSelected.SetActive(false);

            if (defenceSelected.activeSelf)
                defenceSelected.SetActive(false);

            if (vietnegySelected.activeSelf)
                vietnegySelected.SetActive(false);

            offencePanelCanvas.FadeOut();
            defencePanelCanvas.FadeOut();
            vietnegyPanelCanvas.FadeOut();
        }
        yield return new WaitForSeconds(0.5f);

        RectTransform movingRect = null;
        RectTransform targetRect = rectTransformPanelOnPos;
        Vector2 targetLocalPos = Vector2.zero;

        // Chọn RectTransform phù hợp
        if (index == 0)
        {
            movingRect = offenceBtnCanvas.GetComponent<RectTransform>();
        }
        else if (index == 1)
        {
            movingRect = defenceBtnCanvas.GetComponent<RectTransform>();
        }
        else if (index == 2)
        {
            movingRect = vietnegyfadeCanvas.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogWarning("Invalid index for ButtonMovingOn");
            yield break;
        }

        if (isTurnOn)
            targetLocalPos = ConvertAnchorPosition(movingRect, targetRect);
        else
        {
            if (index == 0)
            {
                targetLocalPos = rectTransformOffencePos;
            }
            else
            if (index == 1)
            {
                targetLocalPos = rectTransformDefencePos;
            }
            else if (index == 2)
            {
                targetLocalPos = rectShieldVietnegryPos;
            }
            else
            {
                Debug.LogWarning("Invalid index for ButtonMovingOn");
                yield break;
            }
        }
        Debug.Log("ButtonMoving... Target local position: " + targetLocalPos);

        while (Vector2.Distance(movingRect.anchoredPosition, targetLocalPos) > 0.1f)
        {
            movingRect.anchoredPosition = Vector2.Lerp(movingRect.anchoredPosition, targetLocalPos, Time.deltaTime * 15f);
            yield return null;
        }

        // Đặt đúng vị trí cuối cùng
        movingRect.anchoredPosition = targetLocalPos;
        // TO DO: Turn on correct panel
        if (isTurnOn)
        {
            if (index == 0)
            {
                offencePanelCanvas.FadeIn();
            }
            else if (index == 1)
            {
                defencePanelCanvas.FadeIn();
            }
            else if (index == 2)
            {
                vietnegyPanelCanvas.FadeIn();
            }
        }
        else
        {
            yield return null;
            Restart();
        }
    }

    Vector2 ConvertAnchorPosition(RectTransform rtA, RectTransform rtB)
    {

        // 1. Lấy parent và kích thước
        RectTransform rtParent = rtA.parent as RectTransform;
        Vector2 parentSize = rtParent.rect.size;

        // 2. Tính anchor trung tâm (trường hợp stretch thì dùng trung bình)
        Vector2 anchorA = (rtA.anchorMin + rtA.anchorMax) * 0.5f;
        Vector2 anchorB = (rtB.anchorMin + rtB.anchorMax) * 0.5f;

        // 3. Tọa độ "world-space" của điểm neo B (pixel trong hệ parent)
        Vector2 worldPosB = rtB.anchoredPosition + Vector2.Scale(anchorB, parentSize);

        // 4. Tọa độ anchoredPosition mới cho A
        Vector2 newAnchoredPosA = worldPosB - Vector2.Scale(anchorA, parentSize);

        // 5. Gán cho rtA
        return newAnchoredPosA;
    }

    private void Restart()
    {
        offenceBtnCanvas.AlphaFull();
        defenceBtnCanvas.AlphaFull();
        vietnegyfadeCanvas.AlphaFull();

        if (offenceSelected.activeSelf)
            offenceSelected.SetActive(false);

        if (defenceSelected.activeSelf)
            defenceSelected.SetActive(false);

        if (vietnegySelected.activeSelf)
            vietnegySelected.SetActive(false);

        offencePanelCanvas.AlphaZero();
        defencePanelCanvas.AlphaZero();
        vietnegyPanelCanvas.AlphaZero();
        isButtonShow = true;
        optionBoardController.isPanelChildActing = false;
    }
}
