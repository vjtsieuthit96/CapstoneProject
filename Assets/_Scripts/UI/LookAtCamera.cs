using Invector.vCamera;
using System.Collections;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera targetCam;
    public vThirdPersonCamera tpsCam;

    IEnumerator AfterStart()
    {
        yield return new WaitForSeconds(0.2f);
        if (tpsCam == null)
        {
            tpsCam = FindObjectOfType<vThirdPersonCamera>();
        }

    }
    private void OnEnable()
    {
        tpsCam = FindObjectOfType<vThirdPersonCamera>();
        if (tpsCam != null)
        {
            targetCam = tpsCam.GetComponentInChildren<Camera>();
        }
    }

    void LateUpdate()
    {
        if (targetCam == null) return;

        transform.LookAt(
            transform.position + targetCam.transform.rotation * Vector3.forward,
            targetCam.transform.rotation * Vector3.up
        );
    }
}
