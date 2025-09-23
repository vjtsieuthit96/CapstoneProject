using UnityEngine;

public class intersectionManager : MonoBehaviour
{
    public GameObject Map1;
    public GameObject Map2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        Map1.SetActive(false);
        Map2.SetActive(true);
    }

    private void Start()
    {
        Map1.SetActive(true);
        Map2.SetActive(false);
    }
}
