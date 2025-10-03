using Unity.Mathematics;
using UnityEngine;

public class intersectionManager : MonoBehaviour
{
    public GameObject Map1;
    public GameObject Map2;
    public EnemySpawner Spawner;
    public GameObject DarkTree;
    public BoxCollider BoxCollider;
    public Vector3 NewCheckPoint = new Vector3(103.56f, 9f, 147.01f);
    public AIDirector AI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Map1.SetActive(false);
            Map2.SetActive(true);
            BoxCollider.enabled = false;
            PlayerRealTimeData.Instance.SetCheckpoint(NewCheckPoint, quaternion.identity);
            AI.EnableDirector();
            AI.enabled = false;
        }
    }

    private void Awake()
    {
        Map1.SetActive(true);
        Map2.SetActive(false);
    }
    //private void Update()
    //{
    //    if(DarkTree.activeSelf)
    //    {
    //        Spawner.canSpawn = true;
    //    }
    //}
}
