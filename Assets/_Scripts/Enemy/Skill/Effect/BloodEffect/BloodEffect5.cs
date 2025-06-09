using UnityEngine;

public class BloodEffect5: MonoBehaviour
{
    [SerializeField] private float deactiveTime;
    private void OnEnable()
    {
        Invoke(nameof(Deactive), deactiveTime);
    }

    private void Deactive()
    {
        PoolManager.Instance.ReturnObject("BloodEF5", this);
    }


}