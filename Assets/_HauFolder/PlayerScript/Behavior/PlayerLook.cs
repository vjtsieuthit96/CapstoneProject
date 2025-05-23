using UnityEngine;

public class PlayerLook : ILookBehavior
{
    public void Look(Vector2 lookinput, ref float rotationX, ref float rotationY, float MouseSensity, Transform PlayerTransform, Transform CameraTransform)
    {
        rotationX = lookinput.x * MouseSensity * Time.deltaTime;
        rotationY = lookinput.y * MouseSensity * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);
        PlayerTransform.rotation = Quaternion.Euler(0f, rotationX, 0f);
        CameraTransform.localRotation = Quaternion.Euler(-rotationY, 0f, 0f);
    }
}
    