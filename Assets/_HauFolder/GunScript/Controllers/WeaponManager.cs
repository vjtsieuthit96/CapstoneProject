using UnityEngine;

public class WeaponManager
{
    private IWeapon CurrentWeapon;


    public void Equip (GunController gun)
    {
        CurrentWeapon = gun;
    }
    public void ManualUpdate()
    {
        CurrentWeapon.ManualUpdate();
    }
    public void OnShoot()
    {
        CurrentWeapon?.OnShoot();
    }

    public void OffShoot()
    {
        CurrentWeapon?.OffShoot();
    }

    public void Reload()
    {
        CurrentWeapon?.Reload();
    }
}
