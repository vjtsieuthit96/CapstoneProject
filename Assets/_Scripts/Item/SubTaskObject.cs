using UnityEngine;

public class SubTaskObject : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private GameObject LLQPrefab;

    private void Start()
    {
        LLQPrefab.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            if (LLQPrefab != null)
            {
                LLQPrefab.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        if (LLQPrefab != null)
        {
            LLQController child = LLQPrefab.GetComponentInChildren<LLQController>(true);
            if (child != null)
            {
                child.FadeDisappear();
            }
        }
    }
}
