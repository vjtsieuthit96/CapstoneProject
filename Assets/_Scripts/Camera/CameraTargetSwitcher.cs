using Invector;
using Invector.vCamera;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetSwitcher : MonoBehaviour
{
    [Header("Danh sách target (nhân vật chính + các object khác)")]
    public List<Transform> targets;

    [SerializeField] private vThirdPersonCamera tpsCamera;
    public int currentIndex = 0;
    private bool hasFoundCamera = false;

    void Start()
    {
        TryFindCamera();
    }

    void Update()
    {
        if (!hasFoundCamera)
        {
            TryFindCamera();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchTarget(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchTarget(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchTarget(2);
    }
    public void SwitchTargetToTransform(Transform target)
    {
        if (tpsCamera == null || target == null) return;

        if (!targets.Contains(target))
        {
            targets.Add(target);
        }

        currentIndex = targets.IndexOf(target);
        tpsCamera.SetMainTarget(target);
    }

    private void TryFindCamera()
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("camera");

        foreach (GameObject camObj in cameras)
        {
            vThirdPersonCamera foundCam = camObj.GetComponent<vThirdPersonCamera>();
            if (foundCam != null)
            {
                tpsCamera = foundCam;
                targets.Add(tpsCamera.mainTarget);
                hasFoundCamera = true;
                return;
            }
        }
    }

    public void SwitchTarget(int index)
    {
        if (tpsCamera == null) return;
        if (index < 0 || index >= targets.Count) return;
        if (targets[index] == null) return;

        currentIndex = index;
        tpsCamera.SetMainTarget(targets[index]);
    }
}
