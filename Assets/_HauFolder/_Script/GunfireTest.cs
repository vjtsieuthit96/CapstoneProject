using UnityEngine;

public class GunfireTest : MonoBehaviour
{
    public ParticleSystem MuzzleFlash;
    public float fireRate = 0.2f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        
        if(Input.GetMouseButton(0) && timer >= fireRate)
        {
            FireParticle();
            timer = 0f;
        }
    }

    void FireParticle()
    {
        Debug.Log("FIRE");
        //if (!MuzzleFlash.isPlaying)
        //{
        //    MuzzleFlash.Play();
        //}
        var emitParams = new ParticleSystem.EmitParams();
        MuzzleFlash.Emit(emitParams, 1);
    }
}
