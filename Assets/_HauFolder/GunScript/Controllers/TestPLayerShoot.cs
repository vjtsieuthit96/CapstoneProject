using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TestPLayerShoot : MonoBehaviour
{
    private GunController Gun;
    private IInputSystem Input;

    //UI Test
    [SerializeField] private Text currentBullet;
    [SerializeField] private Text RemainBullet;

    private void Awake()
    {
        Gun = GetComponent<GunController>();
        Input = new TestInputController();
    }
    private void Update()
    {
        HandleShoot();
        HandleReload();
        Gun.ManualUpdate();
        currentBullet.text = Gun.CurrentAmmo.ToString();
        RemainBullet.text = Gun.TotalAmmoReserve.ToString();

    }
    private void HandleShoot()
    {
        if(Input.IsPress())
        {
            Gun.OnShoot();
        }
        if(Input.IsRelease())
        {
            Gun.OffShoot();
        }
    }    

    private void HandleReload()
    {
        if(Input.IsReload())
        {
            Gun.Reload();
        }
    }
}
