using System.Security;
using UnityEngine;

public class GunfireTest : MonoBehaviour
{
    public ParticleSystem MuzzleFlash;
    private InputSystem_Actions Action;
    public float fireRate = 0.2f;
    private bool isFiring;
    private float timer;

    private void Awake()
    {
        Action = new InputSystem_Actions();
        Action.Player.Shoot.performed += ctx => isFiring = true;
        Action.Player.Shoot.canceled += ctx => isFiring = false;
    }

    private void OnEnable()
    {
        Action.Enable();
    }
    private void OnDisable()
    {
        Action.Disable();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if(isFiring && timer >= fireRate)
        {
            FireParticle();
            timer = 0f;
        }
    }

    void FireParticle()
    {
        Debug.Log("FIRE");
        ParticleSystem[] allparticle = MuzzleFlash.GetComponentsInChildren<ParticleSystem>();
        foreach(var Muzzle in allparticle)
        {
            var emitParams = new ParticleSystem.EmitParams();
            Muzzle.Emit(emitParams, 1);
        }
    }
}
