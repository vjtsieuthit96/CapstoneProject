using UnityEngine;

public class PlayerMovement : IMovementBehavior
{
    public void Move(Rigidbody rb, Vector3 Direction, PlayerData data)
    {
        Vector3 Move = Direction.normalized * data.PlayerDefaultSpeed * Time.deltaTime;
        rb.MovePosition(rb.position +  Move);
    }
}
