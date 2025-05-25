using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public float RecoilStrength;
    public float PlayerStrength;

    public void ApplyRecoil(ref float RotationX, ref float RotationY)
    {
        float pitch = RecoilStrength/ (12 * PlayerStrength);
        float yaw = RecoilStrength / (24 *PlayerStrength);
        Debug.Log("Recoil X: " + pitch + " recoil Y: " + yaw);
        RotationY += pitch;
        RotationX += Random.Range(-yaw,yaw);
    }
}
