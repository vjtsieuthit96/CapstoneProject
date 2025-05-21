using UnityEngine;

public static class GunSystemFactory
{
    public static ShootingMethod GetShootingMethod(GunData data)
    {
        return data.Type switch
        {
            Guntype.Piston => ShootingMethod.Single,
            Guntype.Rifle => ShootingMethod.Both,
            Guntype.Submachine => ShootingMethod.Automatic,
            Guntype.Sniper => ShootingMethod.Single,
            _ => ShootingMethod.Single
        };
    }
}
