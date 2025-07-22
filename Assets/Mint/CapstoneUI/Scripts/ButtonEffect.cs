using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private GameObject hoverBorder;
    [SerializeField] private GameObject hoverPanel;
    [SerializeField] private float scaleFactor = 0.3f;
    private Vector3 currentScale;

    private Image img;
    public Color colorChange;
    [SerializeField] private bool canChangeColor;
    private Color currentColor;
    private void Awake()
    {
        img = GetComponent<Image>();
        if(img != null)
            currentColor = img.color;
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
        if(hoverBorder)
        {
            hoverBorder.SetActive(true);
        }
        if(hoverPanel)
        {
            hoverPanel.SetActive(true);
        }
        if (canChangeColor && img)
        {
            img.color = colorChange;
        }
        SoundMixerManager.Instance.PlaySFXAudio(hoverSound);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale -= Vector3.one * scaleFactor;
        if(hoverBorder)
        {
            hoverBorder.SetActive(false);
        }
        if (hoverPanel)
        {
            hoverPanel.SetActive(false);
        }
        if (canChangeColor && img)
        {
            img.color = currentColor;
        }
    }

    private void Actived()
    {
        if (hoverBorder && hoverBorder.activeSelf)
        {
            hoverBorder.SetActive(false);
        }
        if (hoverPanel && hoverPanel.activeSelf)
        {
            hoverPanel.SetActive(false);
        }
        transform.localScale = currentScale;
    }
}
