using UnityEngine;

public interface IMovementBehavior
{
    void Move(Rigidbody rb, Vector3 Direction, PlayerData data);
}
