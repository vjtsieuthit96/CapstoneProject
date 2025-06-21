using Invector.vShooter;
using UnityEngine;
using UnityEngine.Events;
using Invector.vItemManager;
using static UnityEditor.Progress;

public class WeaponInjector : MonoBehaviour
{
    public CharacterConfigurator characterConfigurator;

    public vInventory inventory;

    private void Awake()
    {
        if (inventory == null)
        {
            Debug.LogWarning("[WeaponInjector] inventory chưa được gán!");
            return;
        }
        inventory.onEquipItem.AddListener(OnEquipItemHandler);
    }
    private void Update()
    {
        OnBulletState();
    }

    private void OnBulletState()
    {
        var weapon = GetComponentInChildren<vShooterWeapon>();
        if (weapon != null)
        {
            if (characterConfigurator != null)
            {
                weapon.isExplosive = characterConfigurator.isExplosive;
                weapon.isEffectMode = characterConfigurator.isEffectMode;
                weapon.PlayerElementClass = characterConfigurator.PlayerElementClass;
                weapon.gunData.FireRate *= characterConfigurator.PlayerFireRate;
                weapon.gunData.ReloadTime = characterConfigurator.ReloadSpeed;
                if(weapon.gunData.GunType == GunType.LongGun)
                {
                    weapon.gunData.ClipSize = characterConfigurator.LongGunClipSize;
                }
                weapon.gunData.RecoilLeft *= characterConfigurator.GunRecoil;
                weapon.gunData.RecoilRight *= characterConfigurator.GunRecoil;
                weapon.gunData.RecoilUp *= characterConfigurator.GunRecoil;
                characterConfigurator.GunType = weapon.gunData.GunType;
                characterConfigurator.CurrentBullet = weapon.ammoCount;
                characterConfigurator.CurrentGunClipSize = weapon.gunData.ClipSize;
            }
        }
    }
    private void OnEquipItemHandler(vEquipArea equipArea, vItem item)
    {
        if (item == null) return;

        var weapon = item.originalObject != null ? item.originalObject.GetComponent<vShooterWeapon>() : null;

        if (weapon != null)
        {
            if (characterConfigurator != null)
            {
                if(weapon.gunData.GunType == GunType.ShortGun)
                {
                    weapon.PlayerDamageMultiplier = characterConfigurator.PlayerDamageMultiplierShortgun;
                }
                else if (weapon.gunData.GunType == GunType.LongGun)
                {
                    weapon.PlayerDamageMultiplier = characterConfigurator.PlayerDamageMultiplierLonggun;
                }
                weapon.isExplosive = characterConfigurator.isExplosive;
            }
            InjectToWeapon(weapon);
        }
    }
    public void addBullet(int bullet)
    {
        var weapon = GetComponentInChildren<vShooterWeapon>();
        weapon.AddAmmo(bullet);
    }


    public void InjectToWeapon(vShooterWeapon weapon)
    {
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.onEquipItem.RemoveListener(OnEquipItemHandler);
        }
    }
}
