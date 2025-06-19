using Invector.Utils;
using UnityEngine;

public class OptionBoardController : MonoBehaviour
{
    [SerializeField] private vFadeCanvas fadeCanvas; // Reference to the vFadeCanvas component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeIn()
    {
        if (fadeCanvas != null)
        {
            fadeCanvas.FadeIn();
        }
    }
    public void FadeOut()
    {
        if (fadeCanvas != null)
        {
            fadeCanvas.AlphaZero();
        }
    }
}
