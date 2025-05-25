using System.Security.Cryptography;
using UnityEngine;

public class RecoilPhysicsCalculator
{
    public static float CalculateRecoilForce(GunData gundata, PlayerData playerdata)
    {
        float bulletSpeed = gundata.BulletSpeed;
        float gunmass = gundata.Weight;
        float playermass = playerdata.PlayerWeight;
        float totalmass = gunmass + playermass;
        float Bulletmass = gundata.BulletMass;
        float Bulletmomentum = Bulletmass * bulletSpeed;
        float RecoilForce = Bulletmomentum / totalmass;
        return RecoilForce;
    }

    public static float CalculateRecoilStrength(GunData gundata, PlayerData playerdata)
    {
        float baserecoil = gundata.Gunrecoil;
        float speedfactor = gundata.BulletSpeed / 500f;
        float gunweightfactor = 1f / (gundata.Weight + 0.1f);
        float playerweightfactor = 1f/Mathf.Max(1f,playerdata.PlayerWeight);
        float strengthmodifier = 1f/ Mathf.Max(1f,playerdata.PlayerStrength);

        float result = baserecoil * speedfactor + gunweightfactor * strengthmodifier * playerweightfactor;
        return Mathf.Clamp(result, 0.2f, 10f);
    }

    public static void ApplyRecoilForce (Rigidbody rb, PlayerData playerdata, GunData gundata, Transform playerTransform)
    {
        float recoilForce = CalculateRecoilForce(gundata, playerdata);
        Vector3 recoilDirection = (-playerTransform.forward).normalized;
        rb.AddForce(recoilDirection * recoilForce, ForceMode.Impulse);
    }

    public static void ApplyRecoilStrength(CameraRecoil camerarecoil, GunData gundata, PlayerData playerdata, ref float RotationX, ref float RotationY)
    {
        float recoilStrength = CalculateRecoilStrength(gundata, playerdata);
        camerarecoil.RecoilStrength = recoilStrength;
        camerarecoil.PlayerStrength = playerdata.PlayerStrength;
        camerarecoil.ApplyRecoil(ref RotationX,ref RotationY);
    }
}
