using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastOutlineShaderGraph : MonoBehaviour
{
    public Camera mainCamera;
    public float rayDistance = 400f;

    public Material outlineEnemy;
    public Material outlineTeammate;
    public Material outlineItem;

    private Renderer lastRenderer;
    private Material[] originalMaterials;

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                // Nếu raycast đang trỏ đến object mới
                if (lastRenderer != rend)
                {
                    RestoreLastRenderer();

                    lastRenderer = rend;
                    originalMaterials = rend.sharedMaterials;

                    Material outlineMaterial = GetOutlineMaterialByTag(hit.collider.tag);
                    if (outlineMaterial != null)
                    {
                        // Tạo mảng mới và thêm outlineMaterial vào cuối
                        Material[] newMats = new Material[originalMaterials.Length + 1];
                        originalMaterials.CopyTo(newMats, 0);
                        newMats[newMats.Length - 1] = outlineMaterial;

                        rend.materials = newMats;
                    }
                }
            }
        }
        else
        {
            RestoreLastRenderer();
        }
    }

    private Material GetOutlineMaterialByTag(string tag)
    {
        switch (tag)
        {
            case "Enemy":
                return outlineEnemy;
            case "Player-Teammate":
                return outlineTeammate;
            case "Item":
                return outlineItem;
            default:
                return null;
        }
    }

    private void RestoreLastRenderer()
    {
        if (lastRenderer != null && originalMaterials != null)
        {
            lastRenderer.materials = originalMaterials;
            lastRenderer = null;
            originalMaterials = null;
        }
    }
}
