using Invector.vShooter;
using UnityEngine;
using UnityEngine.UI;
using Invector.vItemManager;

public class WeaponInjector : MonoBehaviour
{
    public CharacterConfigurator characterConfigurator;
    public vInventory inventory;

    [Header("UI Controller")]
    public UIController uiController;

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
        uiController?.UpdateUI();
    }

    private void OnBulletState()
    {
        var weapon = GetComponentInChildren<vShooterWeapon>();
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Type of Current Gun: " + weapon?.GunType);
        }

        if (weapon != null && characterConfigurator != null)
        {
            weapon.isExplosive = characterConfigurator.isExplosive;
            weapon.isEffectMode = characterConfigurator.isEffectMode;
            weapon.PlayerElementClass = characterConfigurator.PlayerElementClass;
            weapon.shootFrequency *= characterConfigurator.PlayerFireRate;
            weapon.reloadTime = characterConfigurator.ReloadSpeed;

            if (weapon.GunType == GunType.LongGun)
                weapon.clipSize = characterConfigurator.LongGunClipSize;

            weapon.recoilLeft *= characterConfigurator.GunRecoil;
            weapon.recoilRight *= characterConfigurator.GunRecoil;
            weapon.recoilUp *= characterConfigurator.GunRecoil;

            characterConfigurator.GunType = weapon.GunType;
            characterConfigurator.CurrentBullet = weapon.ammoCount;
            characterConfigurator.CurrentGunClipSize = weapon.clipSize;

            weapon.PlayerDamageMultiplier = (weapon.GunType == GunType.ShortGun)
                ? characterConfigurator.PlayerDamageMultiplierShortgun
                : characterConfigurator.PlayerDamageMultiplierLonggun;

            if (uiController != null)
            {
                uiController.currentWeapon = weapon;
            }
        }
    }

    private void OnEquipItemHandler(vEquipArea equipArea, vItem item)
    {
        if (item == null) return;

        var weapon = item.originalObject?.GetComponent<vShooterWeapon>();
        if (weapon != null)
        {
            if (characterConfigurator != null)
            {
                weapon.PlayerDamageMultiplier = (weapon.gunData.GunType == GunType.ShortGun)
                    ? characterConfigurator.PlayerDamageMultiplierShortgun
                    : characterConfigurator.PlayerDamageMultiplierLonggun;

                weapon.isExplosive = characterConfigurator.isExplosive;
                weapon.Gunowner = characterConfigurator.gameObject;
                Debug.Log("Gun's Owner: " + weapon.Gunowner);
            }

            uiController.currentWeapon = weapon;
            InjectToWeapon(weapon);
        }
    }

    public void addBullet(int bullet)
    {
        var weapon = GetComponentInChildren<vShooterWeapon>();
        weapon?.AddAmmo(bullet);
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

    [System.Serializable]
    public class UIController
    {
        [Header("References")]
        public RawImage imageElement;
        public RawImage imageExplosion;
        public CharacterConfigurator characterConfigurator;
        [HideInInspector] public vShooterWeapon currentWeapon;

        [Header("Textures for Elements")]
        public Texture2D noneTexture;
        public Texture2D electricTexture;
        public Texture2D frozenTexture;
        public Texture2D poisonTexture;

        [Header("Explosion Colors Per Element")]
        public Color explosionColorNone = Color.gray;
        public Color explosionColorElectric = Color.blue;
        public Color explosionColorFrozen = Color.cyan;
        public Color explosionColorPoison = Color.green;

        [Header("Disabled Color")]
        public Color disabledColor = Color.gray;

        public void UpdateUI()
        {
            if (currentWeapon == null || characterConfigurator == null) return;

            UpdateElementImage();
            UpdateExplosionImage();
        }

        private void UpdateElementImage()
        {
            switch (currentWeapon.GunElement)
            {
                case Element.Electric:
                    imageElement.texture = electricTexture;
                    break;
                case Element.Frozen:
                    imageElement.texture = frozenTexture;
                    break;
                case Element.Poison:
                    imageElement.texture = poisonTexture;
                    break;
                default:
                    imageElement.texture = noneTexture;
                    break;
            }

            imageElement.color = characterConfigurator.isEffectMode ? Color.white : disabledColor;
        }

        private void UpdateExplosionImage()
        {
            if (!characterConfigurator.isExplosive)
            {
                imageExplosion.color = disabledColor;
                return;
            }

            switch (currentWeapon.GunElement)
            {
                case Element.Electric:
                    imageExplosion.color = explosionColorElectric;
                    break;
                case Element.Frozen:
                    imageExplosion.color = explosionColorFrozen;
                    break;
                case Element.Poison:
                    imageExplosion.color = explosionColorPoison;
                    break;
                default:
                    imageExplosion.color = explosionColorNone;
                    break;
            }
        }
    }
}
