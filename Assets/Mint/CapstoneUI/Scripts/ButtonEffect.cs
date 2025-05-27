using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    // Make Zoom efect on button hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale += Vector3.one * 0.3f;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale -= Vector3.one * 0.3f;
    }
}
