using UnityEngine;
using System.Collections.Generic;
using Invector.Utils;

public class SkillTreeUIManager : MonoBehaviour
{
    // OffenceButton
    [SerializeField] private vFadeCanvas offenceBtnCanvas;
    [SerializeField] private vFadeCanvas defenceBtnCanvas;
    [SerializeField] private vFadeCanvas vietnegyfadeCanvas;
    public SkillTreeSystem skillSystem;
    public Transform nodeContainer;

    private List<SkillNodeButton> buttons = new List<SkillNodeButton>();

    private void OnEnable()
    {
        offenceBtnCanvas.AlphaFull();
        defenceBtnCanvas.AlphaFull();
        vietnegyfadeCanvas.AlphaFull();
        // TODO: hide panels on start
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

        switch(index)
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
                break;
            case 2:
                vietnegyfadeCanvas.FadeIn();
                offenceBtnCanvas.FadeOut();
                defenceBtnCanvas.FadeOut();
                break;
            default:
                Debug.LogWarning("Invalid index for FadeIn");
                break;
        }

    }
}
