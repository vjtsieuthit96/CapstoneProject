using UnityEngine;
using TMPro;
using System.Collections;

public class TextCrawlManager : MonoBehaviour
{
    [Header("UI Target")]
    public TMP_Text textComponent;

    [Header("Typing Settings")]
    [TextArea(5, 10)]
    public string[] paragraphs;

    public float typingSpeed = 0.03f;
    public float delayBetweenParagraphs = 1.5f;

    private void Start()
    {
        textComponent.text = "";
        StartCoroutine(TypeAllParagraphs());
    }

    IEnumerator TypeAllParagraphs()
    {
        for (int i = 0; i < paragraphs.Length; i++)
        {
            yield return StartCoroutine(TypeParagraph(paragraphs[i]));
            yield return new WaitForSeconds(delayBetweenParagraphs);
            textComponent.text = "";
        }
    }

    IEnumerator TypeParagraph(string paragraph)
    {
        textComponent.text = "";

        foreach (char c in paragraph)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
