using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "GunData")]
public class GunData : ScriptableObject
{
    // Ammo
    public float ReloadTime;
    public bool ReloadOnebyOne;
    public int ClipSize;

    // Projetile
    public int DamageClass;
    [Range(0f, 1f)] public float CameraStability; 
    public float RecoilRight;
    public float RecoilLeft;
    public float RecoilUp;

    // Gun Stats
    public Element GunElement;
    public BulletType BulletType;
    public GunType GunType;

}
public enum Element
{
    None,
    Electric,
    Frozen,
    Poison
}
public enum BulletType
{
    None,
    Explosion,
}
public enum GunType
{
    ShortGun,
    LongGun
}
