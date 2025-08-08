using Invector.vCharacterController;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public vThirdPersonInput input;
    public Animator animator;

    private void Awake()
    {
        //input.SetLockAllInput(true);
        //input.SetLockCameraInput(true);

    }

    private void ShutDownCamera()
    {
        input.SetLockAllInput(false);
        input.SetLockCameraInput(false);
        animator.enabled = false;
    }    
}
