using UnityEngine;

public static class JumpForceCalculator
{
    public static float CalculateJumpFore(PlayerData playerdata, GunData gundata)
    {
        float totalWeight = playerdata.PlayerWeight + (gundata != null ? gundata.Weight : 0);
        float jumpforce = (playerdata.PlayerStrength *100)/totalWeight;

        return Mathf.Clamp(jumpforce,3,20);
    }
}
