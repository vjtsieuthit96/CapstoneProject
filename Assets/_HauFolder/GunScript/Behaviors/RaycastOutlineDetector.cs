using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastOutlineDetector : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayDistance = 400f;
    [SerializeField] private OutlineLayerResolver layerResolver;

    private OutlineManager outlineManager;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (layerResolver == null)
        {
            Debug.LogError("OutlineLayerResolver is not assigned!", this);
        }

        outlineManager = new OutlineManager();
    }

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.TryGetComponent(out IOutlineTarget target))
            {
                if (layerResolver.TryGetOutlineLayer(hit.collider.tag, out int outlineLayer))
                {
                    outlineManager.Highlight(target, outlineLayer);
                    return;
                }
            }
        }

        outlineManager.ClearHighlight();
    }
}
