using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public string GunName;
    public float Weight;
    public float Gunrecoil;
    public float BulletSpeed;
    public float RateofFire;
    public float Range;
    public float PhysicalDamage;
    public float MaximumTotalBullet;
    public MagicDamageEffect MagicEffect;
    public Guntype Type;
    public ShootingMethod Method
    {
        get
        {
            switch (Type)
            {
                case Guntype.Piston:
                    return ShootingMethod.Single;
                case Guntype.Rifle:
                    return ShootingMethod.Both;
                case Guntype.Submachine:
                    return ShootingMethod.Automatic;
                case Guntype.Sniper:
                    return ShootingMethod.Single;
                default: return ShootingMethod.Single;
            }
        }
    }
}


public enum Guntype
{
    Piston,
    Rifle,
    Submachine,
    Sniper
}

public enum ShootingMethod
{
    Single,
    Automatic,
    Both
}

public enum MagicDamageEffect
{
    None,
    Fire, 
    Ice,
    Lightning,
    Poison,
    Dark,
    Light,
    DamageOverTime,
    Stun,
}
