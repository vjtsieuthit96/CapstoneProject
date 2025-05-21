using UnityEngine;

public class ShootingAuto : IShootingBehavior
{
    private float nextFireTime;

    public void Tick(GunController controller)
    {
        if(controller.WantsToShoot && Time.time > nextFireTime)
        {
            if(controller.WantsToShoot && Time.time >= nextFireTime)
            {
                if(controller.TryShoot())
                {
                    nextFireTime = Time.time + 1f/controller.Data.RateofFire;
                }
            }
        }
    }
}
