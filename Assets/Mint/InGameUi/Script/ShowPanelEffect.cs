using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanelEffect : MonoBehaviour
{
    
    public Image panelImage; // Reference to the Image component of the panel
    public Image decorImage;
    public Text txt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panelImage.fillAmount = 0f;
        decorImage.fillAmount = 0f;
        StartCoroutine(ShowPanel());
    }

    IEnumerator ShowPanel()
    {
        while(decorImage.fillAmount <=0.99f)
        {
            yield return null;
            decorImage.fillAmount += Time.deltaTime * 0.5f; // Adjust the speed as needed
        }
        decorImage.fillAmount = 1f; // Ensure it reaches full fill amount
        while (panelImage.fillAmount < 0.99f)
        {
            yield return null;
            panelImage.fillAmount += Time.deltaTime * 0.5f; // Adjust the speed as needed
        }
    }
}
