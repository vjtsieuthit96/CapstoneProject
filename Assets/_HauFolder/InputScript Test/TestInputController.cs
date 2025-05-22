using UnityEngine;

public class TestInputController : IInputSystem
{
    private InputSystem_Actions Input;

    public TestInputController()
    {
        this.Input = new InputSystem_Actions();
        Input.Player.Enable();
    }
    public bool IsPress() 
    {
        return Input.Player.Shoot.ReadValue<float>() == 1f; 
    }

    public bool IsRelease() { return Input.Player.Shoot.WasReleasedThisFrame();  }


    public bool IsReload() { return Input.Player.Reload.triggered; }
}
