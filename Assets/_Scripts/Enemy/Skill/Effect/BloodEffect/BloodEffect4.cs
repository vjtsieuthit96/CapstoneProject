using UnityEngine;

public class BloodEffect4 : MonoBehaviour
{
    [SerializeField] private float deactiveTime;
    private void OnEnable()
    {
        Invoke(nameof(Deactive), deactiveTime);         
    }

    private void Deactive()
    {
        PoolManager.Instance.ReturnObject("BloodEF4",this);
    }
    

}