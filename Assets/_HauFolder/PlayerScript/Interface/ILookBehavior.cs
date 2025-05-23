using UnityEngine;

public interface ILookBehavior
{
    void Look(Vector2 lookinput, ref float rotationX, ref float rotationY, float MouseSensity, Transform PlayerTransform, Transform CameraTransform);
}
