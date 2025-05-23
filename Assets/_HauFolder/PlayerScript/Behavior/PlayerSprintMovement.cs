using UnityEngine;

public class PlayerSprintMovement : IMovementBehavior
{
    public void Move(Rigidbody rb, Vector3 Direction, float speed)
    {
        Vector3 move = Direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }
}
