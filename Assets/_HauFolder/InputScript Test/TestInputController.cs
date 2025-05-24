using System;
using UnityEngine;

public class TestInputController : IInputSystem
{
    private InputSystem_Actions Input;

    public TestInputController()
    {
        this.Input = new InputSystem_Actions();
        Input.Player.Enable();
    }
    public bool IsPress() {  return Input.Player.Shoot.ReadValue<float>() == 1f; } // nhấn chuột trái
    public bool IsRelease() { return Input.Player.Shoot.WasReleasedThisFrame();  } // thả chuột trái
    public bool IsReload() { return Input.Player.Reload.triggered; } // nhấn R
    public bool IsSpringting() {return Input.Player.Sprint.ReadValue<float>() > 0f;} //nhấn shift trái
    public Vector2 MoveInput() { return Input.Player.Move.ReadValue<Vector2>(); } // các phím điều hướng
    public Vector2 LookInput() { return Input.Player.Look.ReadValue<Vector2>(); } // chuột
}
