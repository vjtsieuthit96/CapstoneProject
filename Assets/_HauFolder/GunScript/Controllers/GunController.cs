using UnityEngine;

public class GunController : MonoBehaviour, IReloadable, IWeapon
{
    // các file data kéo thả 
    public GunData Data { get; private set; }
    public Transform FirePoint;
    public Transform BulletPoint;
    public ParticleSystem MuzzleParticle;
    public ParticleSystem MuzzleFlash;

    // các interface cấu hình nên cách bắn
    public IShootingBehavior ShootingBehavior { get; private set; }
    public IShooter Shooter { get; private set; }

    // các chỉ số của súng
    public int CurrentAmmo; /*{  get; private set; }*/
    public float TotalAmmoReserve; /*{ get; private set; }*/

    // biến bool kích hoạt bắn
    public bool WantsToShoot {  get; private set; }
    public bool Canshoot => CurrentAmmo > 0;
    // hàm khởi tạo của súng 
    public void Initialize(GunData data, IShootingBehavior behavior, IShooter shooter )
    {
        this.Data = data;
        this.ShootingBehavior = behavior;
        this.Shooter = shooter;

        CurrentAmmo = data.MaxAmmoPerMagazine;
        TotalAmmoReserve = data.MaximumTotalBullet;
    }
    public void ManualUpdate()
    {
        ShootingBehavior?.Tick(this);
    }

    public bool TryShoot()
    {
        if (CurrentAmmo <= 0)
        {
            Debug.Log("Hết đạn");
            return false;
        }
        //Debug.Log("Bắn");
        CurrentAmmo--;
        Shooter?.Shoot(this);
        return true;    
    }

    public void Reload()
    {
        int needed = Data.MaxAmmoPerMagazine - CurrentAmmo;
        if (needed <= 0 || TotalAmmoReserve <= 0)
        {
            return;
        }
        int ammoToLoad = Mathf.Min(needed, (int)TotalAmmoReserve);
        CurrentAmmo += ammoToLoad;
        TotalAmmoReserve -= ammoToLoad;
    }

    public void OnShoot()
    {
        WantsToShoot = true;
    }    

    public void OffShoot()
    {
        WantsToShoot = false;
    }    
}

