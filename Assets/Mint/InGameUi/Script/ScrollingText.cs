using System.Collections;
using TMPro;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private float scrollSpeed = 0.0f;
    private int currentDisplayText = 0;
    private string[] textInfo;
    private void OnDisable()
    {
        StopAllCoroutines();
        textMeshPro.text = "";
    }
    public void ActivateText(string text)
    {
        StopAllCoroutines();
        if(audioSource)
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
        if (audioSource && typingSound)
        {
            
            audioSource.clip = typingSound;
            audioSource.Play();
        }
       
        for (int i = 1; i <= totalChars; i++)
        {
            textMeshPro.maxVisibleCharacters = i;
            if(scrollSpeed > 0.0f)
            {
                yield return new WaitForSeconds(scrollSpeed);
            }
            else
            {
                yield return null;
            }
        }
        if (audioSource)
            audioSource.Stop();
    }

}
