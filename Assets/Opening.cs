using UnityEngine;
using Cinemachine;

public class Opening : MonoBehaviour
{
    public CinemachineVirtualCamera Camera1;
    public CinemachineVirtualCamera Camera2;

    private void Awake()
    {
        Camera1.Priority = 20;
        Camera2.Priority = 10;
    }
    public void CameraChange()
    {
        Camera2.Priority = 20;
        Camera1.Priority = 10;
    }
}
