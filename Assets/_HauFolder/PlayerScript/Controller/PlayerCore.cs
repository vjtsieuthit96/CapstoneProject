using UnityEngine;

public class PlayerCore
{
    private GunController CurrentGun;
    private IInputSystem Input;
    //private IMovementBehavior Move;
    private PlayerStateManager State;
    private ILookBehavior Look;
    private Rigidbody Rb;
    private PlayerData Data;
    public PlayerCore (IInputSystem input, PlayerStateManager state, ILookBehavior look, Rigidbody rb, PlayerData data)
    {
        this.Input = input;
        //this.Move = move;
        this.Look = look;
        this.Rb = rb;
        this.Data = data;
        this.State = state;
    }

    public void PlayerMove(Vector3 Direction)
    {
       State.PlayerHandleMovement(Direction);
    }

    public void PlayerLook(Vector2 Inputlook, ref float RotationX, ref float RotationY, Transform PlayerTransform, Transform CameraTransform)
    {
        Look.Look(Inputlook, ref RotationX, ref RotationY, Data.PlayerSensity, PlayerTransform, CameraTransform);
    }
}
