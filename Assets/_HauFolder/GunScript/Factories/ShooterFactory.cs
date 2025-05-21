using UnityEngine;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hit, controller.Data.Range))
        {
            if(hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(controller.Data.PhysicalDamage);
            }
        }


        var muzzle = controller.MuzzleParticle;
        var main = muzzle.main;
        main.startSpeed = controller.Data.BulletSpeed;
        main.startLifetime = controller.Data.Range / controller.Data.BulletSpeed;
        muzzle.Emit(1);
    }
}
