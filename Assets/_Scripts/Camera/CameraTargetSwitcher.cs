using Invector;
using Invector.vCamera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetSwitcher : MonoBehaviour
{
    [Header("Danh sách target (nhân vật chính + các object khác)")]
    public List<Transform> targets = new List<Transform>();

    [SerializeField] private vThirdPersonCamera tpsCamera;
    public int currentIndex = 0;

    void Start()
    {
        //if (tpsCamera == null)
        //    tpsCamera = FindObjectOfType<vThirdPersonCamera>();

        //if (tpsCamera != null && tpsCamera.mainTarget != null)
        //{
        //    if (!targets.Contains(tpsCamera.mainTarget))
        //    {
        //        targets.Insert(0, tpsCamera.mainTarget);
        //    }
        //}
        StartCoroutine(AfterStart());
    }
    IEnumerator AfterStart()
    {
        yield return new WaitForSeconds(0.2f);
        if (tpsCamera == null)
        {
            tpsCamera = FindObjectOfType<vThirdPersonCamera>();
            if (tpsCamera != null && tpsCamera.mainTarget != null)
            {
                if (!targets.Contains(tpsCamera.mainTarget))
                {
                    targets.Insert(0, tpsCamera.mainTarget);
                }
            }
        }    
           
    }
    void Update()
    {
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

    public void SwitchTarget(int index)
    {
        if (tpsCamera == null) return;
        if (index < 0 || index >= targets.Count) return;
        if (targets[index] == null) return;

        currentIndex = index;
        tpsCamera.SetMainTarget(targets[index]);
        Debug.Log($"📷 Camera switched to: {targets[index].name} (index {index})");
    }
}
