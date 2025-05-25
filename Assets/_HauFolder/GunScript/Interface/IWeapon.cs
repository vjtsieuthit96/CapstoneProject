using UnityEngine;

public interface IWeapon
{
    void OnShoot(); // nhấn phím
    void OffShoot(); // nhả phím
    void Reload(); // nạp đạn 
    void ManualUpdate();
    bool Canshoot { get; }
    GunData Data { get; }
}
