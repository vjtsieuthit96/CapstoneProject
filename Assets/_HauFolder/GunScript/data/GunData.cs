using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    // Tên súng
    public string GunName;
    // Khối lượng súng => dự kiến sẽ + khối lượng vào nhân vật để tính tốc độ chạy hiệu quả
    public float Weight;
    // Độ giật -> giảm độ chính xác bắn -> tạo hiệu ứng chân thực
    public float Gunrecoil;
    // Tốc độ đạn, dùng để tính tốc độ chỉnh của đạn, tính thời gian chính xác đạn đến đích để gọi takedamage chính xác
    public float BulletSpeed;
    // Tốc độ bắn của súng (m/s)
    public float RateofFire;
    // Tầm bắn, tương tự với BulletSpeed, dùng để tính dự kiến thời điểm đạn chạm vào object để gọi takedamage hiệu quả
    public float Range;
    // sát thương vật lí
    public float PhysicalDamage;
    // Lượng đạn tối đa
    public float MaximumTotalBullet;
    // Số lượng băn đạn
    public int MaxAmmoPerMagazine;
    // các enum tùy chọn cấu hình súng
    public MagicDamageEffect MagicEffect;
    public Guntype Type;
    public ShootingMethod ShootingMethod;
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
