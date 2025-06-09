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
                weapon.isPhysicsDamage = characterConfigurator.isPhysicsDamage;
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
                weapon.PlayerDamageMultiplier = characterConfigurator.PlayerDamageMultiplier;
                weapon.isExplosive = characterConfigurator.isExplosive;
            }
            InjectToWeapon(weapon);
        }
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
