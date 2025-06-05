using Invector.vShooter;
using UnityEngine;
using UnityEngine.Events;
using Invector.vItemManager;

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

    private void OnEquipItemHandler(vEquipArea equipArea, vItem item)
    {
        if (item == null) return;

        var weapon = item.originalObject != null ? item.originalObject.GetComponent<vShooterWeapon>() : null;

        if (weapon != null)
        {
            Debug.Log("[WeaponInjector] Đổi súng mới: " + weapon.name);
            if (characterConfigurator != null)
            {
                weapon.PlayerDamageMultiplier = characterConfigurator.PlayerDamageMultiplier;
                Debug.Log($"[WeaponInjector] Đã set PlayerDamageMultiplier = {weapon.PlayerDamageMultiplier} cho súng {weapon.name}");
            }
            else
            {
                Debug.LogWarning("[WeaponInjector] characterConfigurator chưa được gán!");
            }

            InjectToWeapon(weapon);
        }
        else
        {
            Debug.Log("[WeaponInjector] Item được trang bị không phải súng.");
        }
    }

    public void InjectToWeapon(vShooterWeapon weapon)
    {
        Debug.Log("[WeaponInjector] Đã nhận được súng mới: " + weapon.name);
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.onEquipItem.RemoveListener(OnEquipItemHandler);
        }
    }
}
