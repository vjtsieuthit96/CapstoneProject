using UnityEngine;
using UnityEngine.AI;

public class DragonFootAligner : MonoBehaviour
{
    [Header("Chân rồng")]
    [SerializeField] private Transform[] dragonFeet; // Gắn 4 chân vào đây trong Inspector

    [Header("NavMesh & Terrain")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float raycastLength = 10f;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if(agent.baseOffset <= 0.5)
        AlignFeetToTerrain();
    }

    private void AlignFeetToTerrain()
    {
        float maxOffset = 0f;

        foreach (Transform foot in dragonFeet)
        {
            Vector3 rayOrigin = foot.position + Vector3.up * 0.5f; // Ray bắt đầu từ trên chân một chút
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastLength, terrainLayer))
            {
                float offset = hit.distance;
                if (offset > maxOffset)
                {
                    maxOffset = offset;
                }
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy mặt đất dưới chân: {foot.name}");
            }
        }

        // Dịch toàn bộ rồng xuống để chân cao nhất chạm đất
        transform.position -= new Vector3(0, maxOffset, 0);

        // Cập nhật lại vị trí NavMeshAgent nếu cần
        if (agent != null)
        {
            agent.Warp(transform.position); // Đảm bảo agent không bị lệch khỏi NavMesh
        }

        Debug.Log($"🐉 Rồng đã được căn chỉnh xuống {maxOffset:F2} đơn vị để chân chạm đất.");
    }
}