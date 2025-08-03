using System.Collections;
using TMPro;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typingSound;
    private int currentDisplayText = 0;
    private string[] textInfo;
    public void ActivateText(string text)
    {
        StopAllCoroutines();
        audioSource.Stop();
        textMeshPro.text = "";
        StartCoroutine(AnimatedText(text));
    }

    IEnumerator AnimatedText(string fullText)
    {
        textMeshPro.text = fullText;
        textMeshPro.ForceMeshUpdate();

        int totalChars = textMeshPro.textInfo.characterCount;
        textMeshPro.maxVisibleCharacters = 0;
        audioSource.clip = typingSound;
        audioSource.Play();
        for (int i = 1; i <= totalChars; i++)
        {
            textMeshPro.maxVisibleCharacters = i;
            yield return null;
        }
        audioSource.Stop();
    }

}
