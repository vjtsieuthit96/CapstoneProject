using UnityEngine;

public class GunSetupComponent : MonoBehaviour
{
    public GunData data;

    private void Awake()
    {
        var controller = GetComponent<GunController>();
        var method = GunSystemFactory.GetShootingMethod(data);
        controller.Initialize(data,
            ShootingBehaviorFactory.CreateBehavior(method),
            ShooterFactory.CreateShooter());
    }
}
