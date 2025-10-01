using UnityEngine;

public class TurnOnBoss : MonoBehaviour
{
    public GameObject BossObject;
    private void Start()
    {
        BossObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        BossObject.SetActive(true);
    }
}
