using UnityEngine;

public class EnemyInstance : MonoBehaviour
{
    public string poolKey;
    public System.Action<EnemyInstance> onDeath;

    public void Die()
    {
        onDeath?.Invoke(this);
        GameObjectPoolManager.Instance.ReturnObject(poolKey, gameObject);
    }
}
