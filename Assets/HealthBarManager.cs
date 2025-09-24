using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField]private BossHealthBar bossHealthBar;

    public void Start()
    {
        bossHealthBar.HealthBarOn();
    }
}


