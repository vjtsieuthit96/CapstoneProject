using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private GameObject hoverPanel;
    [SerializeField] private float scaleFactor = 0.3f;
    private Vector3 currentScale;
    private void Awake()
    {
        currentScale = transform.localScale;
        Actived();
    }
    private void OnEnable()
    {
        // Reset the scale when the button is enabled
        Actived();
    }
    // Make Zoom efect on button hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale += Vector3.one * scaleFactor;
        if(hoverPanel)
        {
            hoverPanel.SetActive(true);
        }
        SoundMixerManager.Instance.PlaySFXAudio(hoverSound);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale -= Vector3.one * scaleFactor;
        if(hoverPanel)
        {
            hoverPanel.SetActive(false);
        }
    }

    private void Actived()
    {
        if (hoverPanel && hoverPanel.activeSelf)
        {
            hoverPanel.SetActive(false);
        }
        transform.localScale = currentScale;
    }
}
