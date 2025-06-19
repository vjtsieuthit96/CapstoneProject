using Invector.Utils;
using UnityEngine;

public enum PanelType
{
    None,
    Option,
    Setting
}
public class UIPanel : MonoBehaviour
{
   
    public PanelType panelType = PanelType.None; // Enum to define the type of panel
    private vFadeCanvas fadeCanvas; // Reference to the vFadeCanvas component
    public AudioClip clickSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeCanvas = GetComponent<vFadeCanvas>(); // Try to get the vFadeCanvas component from the GameObject this script is attached to
        if(fadeCanvas == null)
        {
            Debug.LogError("vFadeCanvas component not found on this GameObject." + gameObject.name); // Log an error if the vFadeCanvas component is not found
        }
    }

   public void FadeIn()
    {
        if (fadeCanvas != null)
        {
            fadeCanvas.FadeIn(); // Call the FadeIn method on the vFadeCanvas component
        }
    }
    public void FadeOut()
    {
        if (fadeCanvas != null)
        {
            fadeCanvas.AlphaZero(); // Call the AlphaZero method on the vFadeCanvas component
        }
    }
}
