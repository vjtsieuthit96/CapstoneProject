using UnityEngine;

public class ShootingBehaviorFactory
{
    public static IShootingBehavior CreateBehavior(ShootingMethod Method)
    {
        return Method switch
        {
            ShootingMethod.Single => new ShootingSingle(),
            ShootingMethod.Automatic => new ShootingAuto(),
            ShootingMethod.Both => new ShootingSingle(),
            _=> new ShootingSingle()
        };
    }

}
