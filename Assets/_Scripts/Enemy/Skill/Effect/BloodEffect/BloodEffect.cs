using UnityEngine;

public class BloodEffect : MonoBehaviour
{ 
    private void OnEnable()
    {
        Invoke(nameof(Deactive), 3f); 
    }

    private void Deactive()
    {
        PoolManager.Instance.ReturnObject("BloodEF",this);
    }

}