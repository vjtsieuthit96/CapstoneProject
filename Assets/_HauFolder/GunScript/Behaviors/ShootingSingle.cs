using UnityEngine;

public class ShootingSingle : IShootingBehavior
{
    private bool canShoot = false;
    public void Tick(GunController controller)
    {
        if(controller.WantsToShoot && canShoot)
        {
            controller.TryShoot();
            canShoot = false;
        }
        if(!controller.WantsToShoot)
        {
            canShoot = true;
        }
    }
}
