using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] private AudioClip hoverSound;
    private Vector3 currentScale;
    private void Awake()
    {
        currentScale = transform.localScale;
    }
    private void OnEnable()
    {
        // Reset the scale when the button is enabled
        transform.localScale = currentScale;
    }
    // Make Zoom efect on button hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale += Vector3.one * 0.3f;
        SoundMixerManager.Instance.PlaySFXAudio(hoverSound);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale -= Vector3.one * 0.3f;
    }
}
