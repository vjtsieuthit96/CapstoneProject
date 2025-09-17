using Invector.vCharacterController;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public vThirdPersonInput input;
    public Animator animator;
    public Camera mycamera;
    private void Start()
    {
        input.SetLockAllInput(true);
        input.SetLockCameraInput(true);
    }

    private void ShutDownCamera()
    {
        input.SetLockAllInput(false);
        input.SetLockCameraInput(false);
        animator.enabled = false;
        mycamera.enabled = false;
    }    
}
