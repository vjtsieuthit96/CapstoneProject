using UnityEngine;

public class PlayerJump : IJumpable
{
    public void Jump(Rigidbody rb, float Force)
    {
        rb.AddForce(Vector3.up * Force, ForceMode.Impulse);
    }
}
