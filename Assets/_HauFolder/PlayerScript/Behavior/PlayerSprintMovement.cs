using System.Data.Common;
using UnityEngine;

public class PlayerSprintMovement : IMovementBehavior
{
    public void Move(Rigidbody rb, Vector3 Direction, PlayerData data)
    {
        float SprintSpeed = data.PlayerDefaultSpeed * data.RatioRun;
        Vector3 move = Direction.normalized * SprintSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }
}
