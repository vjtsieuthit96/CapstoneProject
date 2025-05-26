using UnityEngine;

[DisallowMultipleComponent]
public class OutlineTarget : MonoBehaviour, IOutlineTarget
{
    private int defaultLayer;

    private void Awake()
    {
        defaultLayer = gameObject.layer;
    }

    public void EnableOutline(int outlineLayer)
    {
        gameObject.layer = outlineLayer;
    }

    public void DisableOutline()
    {
        gameObject.layer = defaultLayer;
    }
}
