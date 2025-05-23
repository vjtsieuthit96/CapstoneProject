using UnityEngine;

public class PlayerMovement : IMovementBehavior
{
    public void Move(Rigidbody rb, Vector3 Direction, float speed)
    {
        Vector3 Move = Direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position +  Move);
    }
}
