using UnityEngine;

public interface IShoot
{
    public void Shooting(ParticleSystem particle, GunData gunData, Transform Firepoint, Transform BulletPoint);
}
