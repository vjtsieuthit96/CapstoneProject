using Invector.vCharacterController;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public vThirdPersonInput input;

    private void Awake()
    {
        input.SetLockAllInput(true);
        input.SetLockCameraInput(true);

    }

    private void ShutDownCamera()
    {
        input.SetLockAllInput(false);
        input.SetLockCameraInput(false);
        this.gameObject.SetActive(false);
    }    
}
