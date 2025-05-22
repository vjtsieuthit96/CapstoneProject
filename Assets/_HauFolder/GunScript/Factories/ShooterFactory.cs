using UnityEngine;
using UnityEngine.InputSystem;

public static class ShooterFactory
{
    public static IShooter CreateShooter()
    {
        return new DefaultShooter();
    }
}

public class DefaultShooter : IShooter
{
    public void Shoot(GunController controller)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out var hit, controller.Data.Range))
        {
            if(hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(controller.Data.PhysicalDamage);
            }
        }

        controller.MuzzleFlash?.Emit(1);
        var muzzle = controller.MuzzleParticle;
        var main = muzzle.main;
        main.startSpeed = controller.Data.BulletSpeed;
        main.startLifetime = controller.Data.Range / controller.Data.BulletSpeed;
        muzzle.Emit(1);
    }
}
