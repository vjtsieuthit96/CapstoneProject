using UnityEngine;

public class PlayerCore
{
    private IInputSystem Input;
    private PlayerStateManager State;
    private ILookBehavior Look;
    private WeaponManager weaponManager;
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
        this.weaponManager = new WeaponManager();
    }

    public void PlayerMove(Vector3 Direction)
    {
       State.PlayerHandleMovement(Direction);
    }

    public void PlayerLook(Vector2 Inputlook, ref float RotationX, ref float RotationY, Transform PlayerTransform, Transform CameraTransform)
    {
        Look.Look(Inputlook, ref RotationX, ref RotationY, Data.PlayerSensity, PlayerTransform, CameraTransform);
    }

    public void Equip(GunController gun)
    {
        weaponManager.Equip(gun);
    }

    public void OnShoot()
    {
        weaponManager.OnShoot();
        
    }

    public void GunEffect(GunData gunData, Transform PlayerTransform, CameraRecoil cameraRecoil, ref float RotationX, ref float RotationY)
    {
        RecoilPhysicsCalculator.ApplyRecoilForce(Rb, Data, gunData, PlayerTransform);
        RecoilPhysicsCalculator.ApplyRecoilStrength(cameraRecoil, gunData, Data, ref RotationX, ref RotationY);
    }

    public void OffShoot()
    {
        weaponManager.OffShoot();
    }

    public void Reload()
    {
        weaponManager.Reload();
    }
    public void ManualUpdate()
    {
        weaponManager.ManualUpdate();
    }

}
