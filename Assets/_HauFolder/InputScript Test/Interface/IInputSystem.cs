using UnityEngine;

public interface IInputSystem
{
    bool IsPress();
    bool IsRelease();
    bool IsReload();
    bool IsSpringting();
    Vector2 MoveInput();
    Vector2 LookInput();
}
