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
        Debug.DrawRay(ray.origin, ray.direction * controller.Data.Range, Color.green, 1.0f);
        if(Physics.Raycast(ray, out var hit, controller.Data.Range))
        {
            if(hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(controller.Data.PhysicalDamage);
            }
            Debug.Log(hit.collider.name);
            Vector3 DirectionToTarget = hit.point - controller.BulletPoint.position;
            controller.BulletPoint.forward = DirectionToTarget;
        }

        controller.MuzzleFlash?.Emit(1);
        var muzzle = controller.MuzzleParticle;
        var main = muzzle.main;
        main.startSpeed = controller.Data.BulletSpeed;
        main.startLifetime = controller.Data.Range / controller.Data.BulletSpeed;
        muzzle.Emit(1);
    }
}
