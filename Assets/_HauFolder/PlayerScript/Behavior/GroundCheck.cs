using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform[] groundCheckPoint;
    [SerializeField] private float GroundCheckRadius;
    [SerializeField] private LayerMask layerCheck;
    public bool IsGrounded
    {
        get
        {
            foreach (Transform t in groundCheckPoint)
            {
                if(Physics.CheckSphere(t.position, GroundCheckRadius, layerCheck))
                {
                    return true;
                }    

            }   
            return false;
        }
    }    
}
