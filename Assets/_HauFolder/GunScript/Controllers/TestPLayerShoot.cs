using UnityEngine;
using UnityEngine.Rendering;

public class TestPLayerShoot : MonoBehaviour
{
    private GunController Gun;
    private IInputSystem Input;

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
