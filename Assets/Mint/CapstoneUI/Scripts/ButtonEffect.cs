using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    [Header("Sound")]
    [SerializeField] private AudioClip hoverSound;
    [Header("Hover Effect")]
    [SerializeField] private GameObject hoverBorder;
    [SerializeField] private Image hoverPanel;
    private bool isHovering = false;
    [SerializeField] private float scaleFactor = 0.3f;
    private Vector3 currentScale;

    [SerializeField] private bool haveShowEffect;
    private Color currentColor;
    [SerializeField] bool canChangeImage = false;
    [SerializeField] private Sprite hoverImage;
    [SerializeField] private Sprite normalImage;
    private Image imageComponent;

    private void Awake()
    {
        currentScale = transform.localScale;
    }
    private void Start()
    {
        if(canChangeImage)
        {
            imageComponent = GetComponent<Image>();
            if (imageComponent != null && normalImage != null)
            {
                imageComponent.sprite = normalImage;
            }
        }
        Actived();
    }
    private void OnEnable()
    {
        // Reset the scale when the button is enabled
        Actived();
    }

    private void Update()
    {
    }
    // Make Zoom efect on button hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale += Vector3.one * scaleFactor;
        if(canChangeImage && imageComponent != null && hoverImage != null)
        {
            imageComponent.sprite = hoverImage;
        }
        if (hoverBorder)
        {
            hoverBorder.SetActive(true);
        }
        if (hoverPanel)
        {
            if (haveShowEffect)
                isHovering = true;
            else
                hoverPanel.gameObject.SetActive(true);
        }
            SoundMixerManager.Instance.PlaySFXAudio(hoverSound);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Actived();
    }

    private void Actived()
    {
        if(canChangeImage && normalImage)
            imageComponent.sprite = normalImage;
        if (hoverBorder && hoverBorder.activeSelf)
        {
            hoverBorder.SetActive(false);
        }
        if (hoverPanel)
        {
            if (hoverPanel)
            {
                if (haveShowEffect)
                {
                    isHovering = false;
                    HideHoverPanel();
                }
                else
                    hoverPanel.gameObject.SetActive(false);
            }
        }
        transform.localScale = currentScale;
    }
    private void HideHoverPanel()
    {
        var img = hoverPanel;
        if (img != null)
        {
            img.color = new Color(1f, 1f, 1f, 0f); // Set the color to transparent
        }
    }
}
