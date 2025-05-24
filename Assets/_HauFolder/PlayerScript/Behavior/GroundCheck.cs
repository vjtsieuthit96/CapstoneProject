using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float GroundCheckRadius;
    [SerializeField] private LayerMask layerCheck;
    public bool IsGrounded => Physics.CheckSphere(groundCheckPoint.position, GroundCheckRadius, layerCheck);
}
